using GenericsAnalyzer.Core;
using GenericsAnalyzer.Core.DataStructures;
using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

// The analyzer should not be run concurrently due to the state that it preserves
#pragma warning disable RS1026 // Enable concurrent execution

namespace GenericsAnalyzer
{
    public class PermittedTypeArgumentAnalyzer : CSharpDiagnosticAnalyzer
    {
        private readonly GenericTypeConstraintInfoCollection genericNames = new GenericTypeConstraintInfoCollection();

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            // Concurrent execution is disabled due to the stateful profile of the analyzer

            // Only executed on *usage* of a generic element
            context.RegisterSyntaxNodeAction(AnalyzeGenericNameOrFunctionCall, SyntaxKind.GenericName, SyntaxKind.IdentifierName);

            var genericSupportedMemberDeclarations = new SyntaxKind[]
            {
                SyntaxKind.MethodDeclaration,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.InterfaceDeclaration,
                SyntaxKind.RecordDeclaration,
                SyntaxKind.DelegateDeclaration,
            };
            context.RegisterSyntaxNodeAction(AnalyzeGenericDeclaration, genericSupportedMemberDeclarations);
        }

        private void AnalyzeGenericNameOrFunctionCall(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var node = context.Node;
            var symbolInfo = semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;
            
            switch (node)
            {
                case IdentifierNameSyntax _
                when symbol is IMethodSymbol methodSymbol:
                    if (!methodSymbol.OriginalDefinition.IsGenericMethod)
                        return;

                    break;

                case GenericNameSyntax genericNameNode:
                    if (genericNameNode.IsUnboundGenericName)
                        return;

                    break;

                default:
                    return;
            }
            
            var originalDefinition = symbol.OriginalDefinition;
            AnalyzeGenericNameDefinition(context, originalDefinition);
            AnalyzeGenericNameUsage(context, symbol, node as GenericNameSyntax);
        }
        private void AnalyzeGenericDeclaration(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var declarationExpressionNode = context.Node as MemberDeclarationSyntax;

            if (!declarationExpressionNode.IsGeneric())
                return;

            var symbol = semanticModel.GetDeclaredSymbol(declarationExpressionNode);
            AnalyzeGenericNameDefinition(context, symbol);
        }

        private void AnalyzeGenericNameDefinition(SyntaxNodeAnalysisContext context, ISymbol declaringSymbol)
        {
            if (genericNames.ContainsInfo(declaringSymbol))
                return;

            var typeParameters = declaringSymbol.GetTypeParameters();
            if (typeParameters.IsEmpty)
                return;

            var semanticModel = context.SemanticModel;

            var typeParameterNameIndexer = typeParameters.ToDictionary(t => t.Name);

            var constraints = new GenericTypeConstraintInfo(typeParameters.Length);
            genericNames[declaringSymbol] = constraints;

            var typeConstraintInheritAttibuteData = new List<AttributeData>();

            for (int i = 0; i < typeParameters.Length; i++)
            {
                var parameter = typeParameters[i];
                var attributes = parameter.GetAttributes();

                var system = new TypeConstraintSystem(parameter);
                InitializeSystem();

                constraints[i] = system;
                var typeDiagnostics = system.AnalyzeFinalizedSystem();
                var finiteTypeCount = system.GetFinitePermittedTypeCount();

                // Re-iterate over the attributes to mark erroneous types
                MarkErroneousTypes();

                void InitializeSystem()
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        var a = attributes[j];
                        var attributeNode = a.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

                        if (!AttributeNeedsProcessing(a))
                            continue;

                        switch (a.AttributeClass.Name)
                        {
                            case nameof(InheritBaseTypeUsageConstraintsAttribute):
                            {
                                if (AnalyzeInheritArgumentAttirbuteUsage(attributeNode, declaringSymbol, parameter, context))
                                    continue;

                                var type = declaringSymbol as INamedTypeSymbol;

                                var inheritedTypes = type.GetBaseTypeAndInterfaces();

                                foreach (var inheritedType in inheritedTypes)
                                {
                                    if (!inheritedType.IsGenericType)
                                        continue;

                                    // Recursively analyze base type definitions
                                    var inheritedTypeOriginalDefinition = inheritedType.OriginalDefinition;
                                    AnalyzeGenericNameDefinition(context, inheritedTypeOriginalDefinition);

                                    var inheritedTypeArguments = inheritedType.TypeArguments;
                                    var inheritedTypeParameters = inheritedType.TypeParameters;

                                    for (int k = 0; k < inheritedTypeArguments.Length; k++)
                                        if (inheritedTypeArguments[k].Name == parameter.Name)
                                        {
                                            var inheritedTypeParameter = inheritedTypeParameters[k];
                                            if (!system.InheritFrom(inheritedTypeParameter, genericNames[inheritedTypeOriginalDefinition][k]))
                                                context.ReportDiagnostic(Diagnostics.CreateGA0022(attributeNode, inheritedTypeParameter));
                                        }
                                }
                                continue;
                            }

                            case nameof(InheritTypeConstraintsAttribute):
                            {
                                // This will be analyzed after the first iteration to ensure all constraints are properly loaded
                                typeConstraintInheritAttibuteData.Add(a);
                                continue;
                            }

                            case nameof(OnlyPermitSpecifiedTypesAttribute):
                            {
                                system.OnlyPermitSpecifiedTypes = true;
                                continue;
                            }
                        }

                        // It is assured that the analyzer cares about the attribute from the base intreface check
                        var rule = ParseAttributeRule(a).Value;

                        // The arguments will be always stored as an array, regardless of their count
                        // If an error is thrown here, common causes could be:
                        // - having forgotten to import a namespace
                        // - accidentally asserting unit test markup code as valid instead of asserting diagnostics
                        system.Add(rule, GetConstraintRuleTypeArguments(a));
                    }
                }
                void MarkErroneousTypes()
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        var a = attributes[j];
                        var attributeSyntaxNode = a.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

                        if (attributeSyntaxNode is null)
                            continue;

                        if (!AttributeNeedsProcessing(a))
                            continue;

                        switch (a.AttributeClass.Name)
                        {
                            case nameof(InheritBaseTypeUsageConstraintsAttribute):
                            {
                                // You will be soon used, don't worry
                                continue;
                            }

                            case nameof(OnlyPermitSpecifiedTypesAttribute):
                            {
                                if (system.HasNoPermittedTypes)
                                    context.ReportDiagnostic(Diagnostics.CreateGA0012(attributeSyntaxNode));
                                continue;
                            }
                        }

                        if (finiteTypeCount == 1)
                            context.ReportDiagnostic(Diagnostics.CreateGA0013(attributeSyntaxNode, parameter));

                        var argumentNodes = attributeSyntaxNode.ArgumentList.Arguments;
                        var typeConstants = GetConstraintRuleTypeArguments(a).ToArray();
                        for (int argumentIndex = 0; argumentIndex < typeConstants.Length; argumentIndex++)
                        {
                            var typeConstant = typeConstants[argumentIndex];
                            var argumentNode = argumentNodes[argumentIndex];

                            var type = typeDiagnostics.GetDiagnosticType(typeConstant);

                            var diagnostic = CreateReportDiagnostic();
                            if (!(diagnostic is null))
                                context.ReportDiagnostic(diagnostic);

                            // "Using a non-static local function is fine."
                            //                              -- Rekkon, 2021
                            Diagnostic CreateReportDiagnostic()
                            {
                                switch (type)
                                {
                                    case TypeConstraintSystemDiagnosticType.Conflicting:
                                        return Diagnostics.CreateGA0002(argumentNode, parameter, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.Duplicate:
                                        return Diagnostics.CreateGA0009(argumentNode, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.InvalidTypeArgument:
                                        return Diagnostics.CreateGA0004(argumentNode, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.ConstrainedTypeArgumentSubstitution:
                                        return Diagnostics.CreateGA0005(argumentNode, typeConstant, parameter);

                                    case TypeConstraintSystemDiagnosticType.RedundantlyPermitted:
                                        return Diagnostics.CreateGA0011(argumentNode, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.RedundantlyProhibited:
                                        return Diagnostics.CreateGA0010(argumentNode, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.ReducibleToConstraintClause:
                                        return Diagnostics.CreateGA0006(argumentNode);

                                    case TypeConstraintSystemDiagnosticType.RedundantBaseTypeRule:
                                        return Diagnostics.CreateGA0008(argumentNode, typeConstant);

                                    case TypeConstraintSystemDiagnosticType.RedundantBoundUnboundRule:
                                        return Diagnostics.CreateGA0003(argumentNode, parameter, typeConstant as INamedTypeSymbol);
                                }
                                return null;
                            }
                        }
                    }
                }
            }
            // Analyze the inherited type constaints from local type parameters
            AnalyzeInheritedTypeConstraints();

            void AnalyzeInheritedTypeConstraints()
            {
                var inheritMap = new Dictionary<ITypeParameterSymbol, TypeParameterAttributeArgumentCorrelationDictionary>(SymbolEqualityComparer.Default);
                var typeParameterInheritanceArguments = new Dictionary<ITypeParameterSymbol, SeparatedSyntaxList<AttributeArgumentSyntax>>(SymbolEqualityComparer.Default);

                foreach (var p in typeParameters)
                {
                    inheritMap.Add(p, new TypeParameterAttributeArgumentCorrelationDictionary());
                    typeParameterInheritanceArguments.Add(p, SyntaxFactory.SeparatedList<AttributeArgumentSyntax>());
                }

                // Create inherit map from attribute data
                foreach (var attributeData in typeConstraintInheritAttibuteData)
                {
                    if (attributeData is null)
                        return;

                    var ctorArguments = attributeData.ConstructorArguments;
                    if (ctorArguments.Length == 0)
                        return;

                    var typeParameterNames = ctorArguments[0].Values.Select(c => c.Value as string).ToArray();

                    // TODO: Validate that this can never be null
                    var attributeNode = attributeData.ApplicationSyntaxReference.GetSyntax(context.CancellationToken) as AttributeSyntax;
                    var arguments = attributeNode.ArgumentList.Arguments;

                    var originalTypeParameterNode = attributeNode.Parent.Parent as TypeParameterSyntax;
                    var originalTypeParameter = semanticModel.GetDeclaredSymbol(originalTypeParameterNode);

                    typeParameterInheritanceArguments[originalTypeParameter] = arguments;

                    for (int j = 0; j < typeParameterNames.Length; j++)
                    {
                        var inheritingTypeParameterName = typeParameterNames[j];
                        var attributeArgumentNode = arguments[j];

                        if (inheritingTypeParameterName == originalTypeParameter.Name)
                        {
                            context.ReportDiagnostic(Diagnostics.CreateGA0021(attributeArgumentNode));
                            continue;
                        }

                        var inheritingTypeParameter = typeParameters.FirstOrDefault(p => p.Name == inheritingTypeParameterName);
                        if (inheritingTypeParameter is null)
                        {
                            context.ReportDiagnostic(Diagnostics.CreateGA0019(attributeArgumentNode, inheritingTypeParameterName));
                            continue;
                        }

                        var originalTypeParameterInherit = inheritMap[originalTypeParameter];
                        originalTypeParameterInherit.Add(inheritingTypeParameter, attributeArgumentNode);
                    }
                }

                // Recursively inherit
                var inheritStack = new StackSet<ITypeParameterSymbol>(inheritMap.Count, SymbolEqualityComparer.Default);
                foreach (var inheritor in inheritMap)
                {
                    var inheritorType = inheritor.Key;
                    var correlationDictionary = inheritor.Value;

                    while (correlationDictionary.Any())
                    {
                        var inheritedTypeCorrelation = correlationDictionary.First();
                        var inheritedType = inheritedTypeCorrelation.Key;

                        bool isRecursiveInheritance = false;

                        // Discover the inheritance stack
                        while (inheritedType != null)
                        {
                            if (isRecursiveInheritance = !inheritStack.Push(inheritorType))
                            {
                                // The diagnostic is always emitted on the source of the recursion
                                context.ReportDiagnostics(inheritedTypeCorrelation.Value, a => Diagnostics.CreateGA0020(a, inheritStack.Reverse()));
                                break;
                            }

                            inheritorType = inheritedType;
                            inheritedTypeCorrelation = inheritMap[inheritorType].FirstOrDefault();
                            inheritedType = inheritedTypeCorrelation.Key;
                        }

                        // Consume the inheritance stack
                        while (!inheritStack.IsEmpty)
                        {
                            inheritedType = inheritorType;
                            inheritorType = inheritStack.Pop();
                            inheritedTypeCorrelation = inheritMap[inheritorType].GetKeyValuePair(inheritedType);

                            if (!isRecursiveInheritance)
                            {
                                // Apply inheritance
                                if (!constraints[inheritorType].InheritFrom(inheritedType, constraints[inheritedType]))
                                {
                                    context.ReportDiagnostics(inheritMap[inheritorType][inheritedType], a => Diagnostics.CreateGA0023(a, inheritedType));
                                }
                            }

                            inheritMap[inheritorType].Remove(inheritedType);
                        }
                    }
                }
            }
        }
        private void AnalyzeGenericNameUsage(SyntaxNodeAnalysisContext context, ISymbol symbol, GenericNameSyntax genericNameNode)
        {
            var originalDefinition = symbol.OriginalDefinition;
            var typeArguments = symbol.GetTypeArguments();

            var typeArgumentNodes = genericNameNode?.TypeArgumentList.Arguments.ToArray();

            var constraints = genericNames[originalDefinition];
            var candidateErrorNode = context.Node;

            for (int i = 0; i < typeArguments.Length; i++)
            {
                if (typeArgumentNodes != null)
                    candidateErrorNode = typeArgumentNodes[i];

                var argumentType = typeArguments[i];
                var system = constraints[i];

                if (argumentType is ITypeParameterSymbol declaredTypeParameter)
                {
                    var declaringElementTypeParameters = declaredTypeParameter.GetDeclaringSymbol().GetTypeParameters();

                    foreach (var declaringElementTypeParameter in declaringElementTypeParameters)
                    {
                        if (declaringElementTypeParameter.Name == declaredTypeParameter.Name)
                        {
                            if (!system.IsPermitted(declaredTypeParameter, genericNames))
                                context.ReportDiagnostic(Diagnostics.CreateGA0017(candidateErrorNode, originalDefinition, argumentType));
                            
                            break;
                        }
                    }
                }
                else
                {
                    if (!system.IsPermitted(argumentType))
                        context.ReportDiagnostic(Diagnostics.CreateGA0001(candidateErrorNode, originalDefinition, argumentType));
                }
            }
        }

        // Emits GA0014, GA0015, GA0016
        private bool AnalyzeInheritArgumentAttirbuteUsage(AttributeSyntax attributeNode, ISymbol symbol, ITypeParameterSymbol parameter, SyntaxNodeAnalysisContext context)
        {
            if (attributeNode is null)
                return false;

            var type = symbol as INamedTypeSymbol;

            if (symbol is IMethodSymbol || !type.TypeKind.CanInheritTypes())
            {
                context.ReportDiagnostic(Diagnostics.CreateGA0014(attributeNode, symbol));
                return true;
            }

            var allBaseTypes = type.GetBaseTypeAndDirectInterfaces().ToImmutableArray();
            var allGenericBaseTypes = allBaseTypes.Where(t => t.IsGenericType).ToImmutableArray();

            if (!allGenericBaseTypes.Any())
            {
                context.ReportDiagnostic(Diagnostics.CreateGA0015(attributeNode, symbol));
                return true;
            }

            bool typeUsedInBaseTypes = false;
            foreach (var baseType in allGenericBaseTypes)
            {
                // The type has type arguments substitute the base type's type parameters
                if (baseType.TypeArguments.Any(arg => arg.Name == parameter.Name))
                {
                    typeUsedInBaseTypes = true;
                    break;
                }
            }

            if (!typeUsedInBaseTypes)
            {
                context.ReportDiagnostic(Diagnostics.CreateGA0016(attributeNode, symbol));
                return true;
            }

            return false;
        }

        private static bool AttributeNeedsProcessing(AttributeData data)
        {
            return data.AttributeClass.IsGenericConstraintAttribute();
        }
        private static IEnumerable<ITypeSymbol> GetConstraintRuleTypeArguments(AttributeData data)
        {
            return data.ConstructorArguments[0].Values.Select(c => c.Value as ITypeSymbol);
        }

        private static TypeConstraintRule? ParseAttributeRule(AttributeData data)
        {
            if (!ConstrainedTypesAttributeBase.ConstrainedTypeAttributeTypes.Any(t => t.Name == data.AttributeClass.Name))
                return null;

            return ConstrainedTypesAttributeBase.GetConstraintRule(data.AttributeClass.Name);
        }
    }
}
