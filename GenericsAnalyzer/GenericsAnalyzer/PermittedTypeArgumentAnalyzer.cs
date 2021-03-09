using GenericsAnalyzer.Core;
using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static GenericsAnalyzer.DiagnosticDescriptors;

// The analyzer should not be run concurrently due to the state that it preserves 
#pragma warning disable RS1026 // Enable concurrent execution

namespace GenericsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PermittedTypeArgumentAnalyzer : DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics = new[]
        {
            GA0001_Rule,
            GA0002_Rule,
            GA0009_Rule,
            GA0014_Rule,
            GA0015_Rule,
            GA0016_Rule,
            GA0017_Rule,
        }.ToImmutableArray();

        private readonly GenericTypeConstraintInfoCollection genericNames = new GenericTypeConstraintInfoCollection();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => supportedDiagnostics;

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

        // Okay this needs some serious refactoring
        private void AnalyzeGenericNameDefinition(SyntaxNodeAnalysisContext context, ISymbol symbol)
        {
            if (genericNames.ContainsInfo(symbol))
                return;

            var typeParameters = symbol.GetTypeParameters();
            if (typeParameters.IsEmpty)
                return;

            var constraints = new GenericTypeConstraintInfo(typeParameters.Length);
            var typeDiagnostics = new TypeConstraintSystemDiagnostics();

            for (int i = 0; i < typeParameters.Length; i++)
            {
                var parameter = typeParameters[i];
                var attributes = parameter.GetAttributes();

                var system = new TypeConstraintSystem();
                for (int j = 0; j < attributes.Length; j++)
                {
                    var a = attributes[j];
                    var attributeSyntaxNode = a.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

                    if (a.AttributeClass.Name == nameof(InheritBaseTypeUsageConstraintsAttribute))
                    {
                        if (AnalyzeInheritArgumentAttirbuteUsage(attributeSyntaxNode, symbol, parameter, context))
                            continue;

                        var type = symbol as INamedTypeSymbol;

                        var inheritedTypes = new List<INamedTypeSymbol>();
                        var baseType = type.BaseType;
                        if (baseType != null)
                            inheritedTypes.Add(baseType);
                        inheritedTypes.AddRange(type.AllInterfaces);

                        foreach (var inheritedType in inheritedTypes)
                        {
                            if (!inheritedType.IsGenericType)
                                continue;

                            // Recursively analyze base type definitions
                            AnalyzeGenericNameDefinition(context, inheritedType);

                            var inheritedTypeParameters = inheritedType.TypeParameters;
                            for (int k = 0; k < inheritedTypeParameters.Length; k++)
                                if (inheritedTypeParameters[k].Name == parameter.Name)
                                    system.InheritFrom(inheritedTypeParameters[k], genericNames[inheritedType][k]);
                        }
                        continue;
                    }

                    if (a.AttributeClass.Name == nameof(OnlyPermitSpecifiedTypesAttribute))
                    {
                        system.OnlyPermitSpecifiedTypes = true;
                        continue;
                    }

                    var rule = ParseAttributeRule(a);
                    if (rule is null)
                        continue;

                    // The arguments will be always stored as an array, regardless of their count
                    // If an error is thrown here, a common cause could be having forgotten to import a namespace
                    system.Add(rule.Value, GetConstraintRuleTypeArguments(a)).RegisterOnto(typeDiagnostics);
                }

                // Re-iterate over the attributes to find conflicting and duplicate types
                for (int j = 0; j < attributes.Length; j++)
                {
                    var a = attributes[j];
                    var attributeSyntaxNode = a.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

                    if (attributeSyntaxNode is null)
                        continue;

                    // Ensure that the attribute is a valid one
                    if (ParseAttributeRule(a) is null)
                        continue;

                    var argumentNodes = attributeSyntaxNode.ArgumentList.Arguments;
                    var typeConstants = GetConstraintRuleTypeArguments(a).ToArray();
                    for (int argumentIndex = 0; argumentIndex < typeConstants.Length; argumentIndex++)
                    {
                        var typeConstant = typeConstants[argumentIndex];
                        var argumentNode = argumentNodes[argumentIndex];

                        var type = typeDiagnostics.GetDiagnosticType(typeConstant);
                        switch (type)
                        {
                            case TypeConstraintSystemDiagnosticType.Conflicting:
                                Diagnostics.CreateGA0002(argumentNode, symbol, typeConstant);
                                break;

                            case TypeConstraintSystemDiagnosticType.Duplicate:
                                Diagnostics.CreateGA0009(argumentNode, typeConstant);
                                break;
                        }
                    }
                }

                constraints[i] = system;
            }

            genericNames[symbol] = constraints;
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
        private bool AnalyzeInheritArgumentAttirbuteUsage(AttributeSyntax attributeSyntaxNode, ISymbol symbol, ITypeParameterSymbol parameter, SyntaxNodeAnalysisContext context)
        {
            if (attributeSyntaxNode is null)
                return false;

            var type = symbol as INamedTypeSymbol;

            if (symbol is IMethodSymbol || !type.TypeKind.CanInheritTypes())
            {
                context.ReportDiagnostic(Diagnostics.CreateGA0014(attributeSyntaxNode, symbol));
                return true;
            }

            var allBaseTypes = type.GetAllDirectBaseTypes().ToImmutableArray();
            var allGenericBaseTypes = allBaseTypes.Where(t => t.IsGenericType).ToImmutableArray();

            if (!allGenericBaseTypes.Any())
            {
                context.ReportDiagnostic(Diagnostics.CreateGA0015(attributeSyntaxNode, symbol));
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
                context.ReportDiagnostic(Diagnostics.CreateGA0016(attributeSyntaxNode, symbol));
                return true;
            }

            return false;
        }

        private static IEnumerable<INamedTypeSymbol> GetConstraintRuleTypeArguments(AttributeData data)
        {
            return data.ConstructorArguments[0].Values.Select(c => c.Value as INamedTypeSymbol);
        }

        private static TypeConstraintRule? ParseAttributeRule(AttributeData data)
        {
            if (!ConstrainedTypesAttributeBase.ConstrainedTypeAttributeTypes.Any(t => t.Name == data.AttributeClass.Name))
                return null;

            return ConstrainedTypesAttributeBase.GetConstraintRule(data.AttributeClass.Name);
        }
    }
}
