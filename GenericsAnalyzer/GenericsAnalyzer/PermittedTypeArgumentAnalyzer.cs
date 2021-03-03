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

namespace GenericsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PermittedTypeArgumentAnalyzer : DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics = new[]
        {
            GA0001_Rule,
            GA0014_Rule,
        }.ToImmutableArray();

        private readonly GenericTypeConstraintInfoCollection genericNames = new GenericTypeConstraintInfoCollection();
        private readonly GenericNameUsageCollection genericTypeUsages = new GenericNameUsageCollection();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => supportedDiagnostics;

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            // Concurrent execution is disabled due to the stateful profile of the analyzer

            // Only executed on *usage* of a generic element
            context.RegisterSyntaxNodeAction(AnalyzeGenericName, SyntaxKind.GenericName);

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

        private void AnalyzeGenericName(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var genericNameNode = context.Node as GenericNameSyntax;
            if (genericNameNode.IsUnboundGenericName)
                return;

            var symbolInfo = semanticModel.GetSymbolInfo(genericNameNode);
            var symbol = symbolInfo.Symbol;

            genericTypeUsages.Register(symbol, genericNameNode);
            var originalDefinition = symbol.OriginalDefinition;
            AnalyzeGenericNameDefinition(context, originalDefinition);
            AnalyzeGenericNameUsage(context, symbol, genericNameNode);
        }
        private void AnalyzeGenericDeclaration(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var declarationExpressionNode = context.Node as MemberDeclarationSyntax;

            // Arity determines how many generic type arguments there are
            // Imagine if only there was an IAritySyntax interface
            switch (declarationExpressionNode)
            {
                case TypeDeclarationSyntax typeDeclaration:
                    if (typeDeclaration.Arity == 0)
                        return;
                    break;
                case DelegateDeclarationSyntax delegateDeclaration:
                    if (delegateDeclaration.Arity == 0)
                        return;
                    break;
                case MethodDeclarationSyntax methodDeclaration:
                    if (methodDeclaration.Arity == 0)
                        return;
                    break;
            }

            var symbol = semanticModel.GetDeclaredSymbol(declarationExpressionNode);
            AnalyzeGenericNameDefinition(context, symbol);
        }

        // Okay this needs some serious refactoring
        private void AnalyzeGenericNameDefinition(SyntaxNodeAnalysisContext context, ISymbol symbol)
        {
            if (genericNames.ContainsInfo(symbol))
                return;

            ImmutableArray<ITypeParameterSymbol> typeParameters;

            // This is in dire need of some better abstraction
            // Say something like IGenericSupportSymbol
            if (symbol is INamedTypeSymbol t)
                typeParameters = t.TypeParameters;
            else if (symbol is IMethodSymbol m)
                typeParameters = m.TypeParameters;
            else
                return;

            var constraints = new GenericTypeConstraintInfo(typeParameters.Length);
            for (int i = 0; i < typeParameters.Length; i++)
            {
                var parameter = typeParameters[i];

                // This truly is ridiculous
                var attributes = parameter.GetAttributes();
                var typeParameterSyntaxNode = parameter.DeclaringSyntaxReferences[0].GetSyntax() as TypeParameterSyntax;
                var lists = typeParameterSyntaxNode.AttributeLists;
                var attributeSyntaxNodes = new List<AttributeSyntax>();
                foreach (var l in lists)
                    attributeSyntaxNodes.AddRange(l.Attributes);

                var system = new TypeConstraintSystem();
                for (int j = 0; j < attributes.Length; j++)
                {
                    var a = attributes[j];
                    var attributeSyntaxNode = attributeSyntaxNodes[j];

                    if (a.AttributeClass.Name == nameof(InheritBaseTypeUsageConstraintsAttribute))
                    {
                        if (!(symbol is INamedTypeSymbol type) || !type.TypeKind.CanInheritTypes())
                        {
                            var diagnostic = Diagnostic.Create(GA0014_Rule, attributeSyntaxNode.GetLocation(), symbol.ToDisplayString());
                            context.ReportDiagnostic(diagnostic);
                            continue;
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
            var semanticModel = context.SemanticModel;

            var originalDefinition = symbol.OriginalDefinition;

            var typeArguments = genericNameNode.TypeArgumentList.Arguments.ToArray();

            var constraints = genericNames[originalDefinition];
            for (int i = 0; i < typeArguments.Length; i++)
            {
                var argument = typeArguments[i];

                var system = constraints[i];
                var argumentType = semanticModel.GetTypeInfo(argument).Type;

                bool isPermitted = false;
                if (argumentType is ITypeParameterSymbol declaredTypeParameter)
                {
                    var declaringElement = declaredTypeParameter.DeclaringMethod;
                    ImmutableArray<ITypeParameterSymbol> declaringElementTypeParameters;
                    if (declaringElement is null)
                        declaringElementTypeParameters = declaredTypeParameter.DeclaringType.TypeParameters;
                    else
                        declaringElementTypeParameters = declaringElement.TypeParameters;

                    for (int j = 0; j < declaringElementTypeParameters.Length; j++)
                    {
                        if (declaringElementTypeParameters[j].Name == declaredTypeParameter.Name)
                        {
                            isPermitted = system.IsPermitted(declaredTypeParameter, j, genericNames);
                            break;
                        }
                    }
                }
                else
                {
                    isPermitted = system.IsPermitted(argumentType);
                }

                if (!isPermitted)
                {
                    var diagnostic = Diagnostic.Create(GA0001_Rule, argument.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
                    context.ReportDiagnostic(diagnostic);
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
