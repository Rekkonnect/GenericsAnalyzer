﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    internal static class Diagnostics
    {
        public static Diagnostic CreateGA0001(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0001_Rule, node?.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0002(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeParameterSymbol typeParameter, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0002_Rule, attributeArgumentSyntaxNode?.GetLocation(), typeParameter.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0003(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeParameterSymbol typeParameter, INamedTypeSymbol genericTypeArgument)
        {
            return Diagnostic.Create(GA0003_Rule, attributeArgumentSyntaxNode?.GetLocation(), typeParameter.ToDisplayString(), genericTypeArgument.ToDisplayString());
        }
        public static Diagnostic CreateGA0004(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0004_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0005(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType, ITypeParameterSymbol typeParameter)
        {
            return Diagnostic.Create(GA0005_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString(), typeParameter.ToDisplayString());
        }
        public static Diagnostic CreateGA0006(AttributeArgumentSyntax attributeArgumentSyntaxNode)
        {
            return Diagnostic.Create(GA0006_Rule, attributeArgumentSyntaxNode?.GetLocation());
        }
        public static Diagnostic CreateGA0008(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0008_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0009(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0009_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0010(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0010_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0011(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0011_Rule, attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0012(AttributeSyntax attributeSyntaxNode)
        {
            return Diagnostic.Create(GA0012_Rule, attributeSyntaxNode?.GetLocation());
        }
        public static Diagnostic CreateGA0013(AttributeSyntax attributeSyntaxNode, ITypeParameterSymbol typeParameter)
        {
            return Diagnostic.Create(GA0013_Rule, attributeSyntaxNode?.GetLocation(), typeParameter.ToDisplayString());
        }
        public static Diagnostic CreateGA0014(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(GA0014_Rule, attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0015(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(GA0015_Rule, attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0016(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(GA0016_Rule, attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0017(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0017_Rule, node?.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0019(AttributeArgumentSyntax attributeArgumentNode, string typeParameterName)
        {
            return Diagnostic.Create(GA0019_Rule, attributeArgumentNode?.GetLocation(), typeParameterName);
        }
        public static Diagnostic CreateGA0020(AttributeArgumentSyntax attributeArgumentNode, IEnumerable<ITypeParameterSymbol> recursionPath)
        {
            return Diagnostic.Create(GA0020_Rule, attributeArgumentNode?.GetLocation(), string.Join(", ", recursionPath.Select(t => t.ToDisplayString())));
        }
        public static Diagnostic CreateGA0021(AttributeArgumentSyntax attributeArgumentNode)
        {
            return Diagnostic.Create(GA0021_Rule, attributeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0022(TypeParameterSyntax typeParameterDeclarationNode)
        {
            return Diagnostic.Create(GA0022_Rule, typeParameterDeclarationNode?.Identifier.GetLocation());
        }
        public static Diagnostic CreateGA0023(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, GA0023_Rule);
        }
        public static Diagnostic CreateGA0024(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, GA0024_Rule);
        }
        public static Diagnostic CreateGA0025(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, GA0025_Rule);
        }
        public static Diagnostic CreateGA0025(BaseTypeSyntax baseTypeNode)
        {
            return Diagnostic.Create(GA0025_Rule, baseTypeNode?.GetLocation());
        }
        public static Diagnostic CreateGA0026(AttributeArgumentSyntax nonProfileTypeArgumentNode)
        {
            return Diagnostic.Create(GA0026_Rule, nonProfileTypeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0027(AttributeArgumentSyntax nonProfileGroupTypeArgumentNode)
        {
            return Diagnostic.Create(GA0027_Rule, nonProfileGroupTypeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0028(AttributeSyntax typeProfileInheritanceAttributeNode)
        {
            return Diagnostic.Create(GA0028_Rule, typeProfileInheritanceAttributeNode?.GetLocation());
        }
        public static Diagnostic CreateGA0029(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, GA0029_Rule);
        }
        public static Diagnostic CreateGA0030(AttributeSyntax attributeNode)
        {
            return Diagnostic.Create(GA0030_Rule, attributeNode?.GetLocation());
        }

        private static Diagnostic CreateProfileInterfaceDiagnostic(InterfaceDeclarationSyntax interfaceDeclarationNode, DiagnosticDescriptor rule)
        {
            return Diagnostic.Create(rule, interfaceDeclarationNode?.Identifier.GetLocation());
        }
    }
}
