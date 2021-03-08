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
            GA0014_Rule,
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
                        var type = symbol as INamedTypeSymbol;

                        if (attributeSyntaxNode != null)
                        {
                            if (type is null || !type.TypeKind.CanInheritTypes())
                            {
                                var diagnostic = Diagnostic.Create(GA0014_Rule, attributeSyntaxNode.GetLocation(), symbol.ToDisplayString());
                                context.ReportDiagnostic(diagnostic);
                                continue;
                            }
                        }

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
                    system.Add(rule.Value, a.ConstructorArguments[0].Values.Select(c => c.Value as INamedTypeSymbol));
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
                if (!(typeArgumentNodes is null))
                    candidateErrorNode = typeArgumentNodes[i];

                var argumentType = typeArguments[i];
                var system = constraints[i];

                if (argumentType is ITypeParameterSymbol declaredTypeParameter)
                {
                    var declaringElementTypeParameters = declaredTypeParameter.GetDeclaringSymbol().GetTypeParameters();

                    for (int j = 0; j < declaringElementTypeParameters.Length; j++)
                    {
                        if (declaringElementTypeParameters[j].Name == declaredTypeParameter.Name)
                        {
                            if (!system.IsPermitted(declaredTypeParameter, j, genericNames))
                            {
                                var diagnostic = Diagnostic.Create(GA0017_Rule, candidateErrorNode.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
                                context.ReportDiagnostic(diagnostic);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (!system.IsPermitted(argumentType))
                    {
                        var diagnostic = Diagnostic.Create(GA0001_Rule, candidateErrorNode.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private static TypeConstraintRule? ParseAttributeRule(AttributeData data)
        {
            if (!ConstrainedTypesAttributeBase.ConstrainedTypeAttributeTypes.Any(t => t.Name == data.AttributeClass.Name))
                return null;

            return ConstrainedTypesAttributeBase.GetConstraintRule(data.AttributeClass.Name);
        }
    }
}
