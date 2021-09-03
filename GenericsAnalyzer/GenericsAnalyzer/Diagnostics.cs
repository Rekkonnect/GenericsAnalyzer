using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static GenericsAnalyzer.GADiagnosticDescriptorStorage;

namespace GenericsAnalyzer
{
    internal static class Diagnostics
    {
        public static Diagnostic CreateGA0001(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0001], node?.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0002(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeParameterSymbol typeParameter, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0002], attributeArgumentSyntaxNode?.GetLocation(), typeParameter.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0003(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeParameterSymbol typeParameter, INamedTypeSymbol genericTypeArgument)
        {
            return Diagnostic.Create(Instance[0003], attributeArgumentSyntaxNode?.GetLocation(), typeParameter.ToDisplayString(), genericTypeArgument.ToDisplayString());
        }
        public static Diagnostic CreateGA0004(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0004], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0005(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType, ITypeParameterSymbol typeParameter)
        {
            return Diagnostic.Create(Instance[0005], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString(), typeParameter.ToDisplayString());
        }
        public static Diagnostic CreateGA0006(AttributeArgumentSyntax attributeArgumentSyntaxNode)
        {
            return Diagnostic.Create(Instance[0006], attributeArgumentSyntaxNode?.GetLocation());
        }
        public static Diagnostic CreateGA0008(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0008], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0009(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0009], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0010(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0010], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0011(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0011], attributeArgumentSyntaxNode?.GetLocation(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0012(AttributeSyntax attributeSyntaxNode)
        {
            return Diagnostic.Create(Instance[0012], attributeSyntaxNode?.GetLocation());
        }
        public static Diagnostic CreateGA0013(AttributeSyntax attributeSyntaxNode, ITypeParameterSymbol typeParameter)
        {
            return Diagnostic.Create(Instance[0013], attributeSyntaxNode?.GetLocation(), typeParameter.ToDisplayString());
        }
        public static Diagnostic CreateGA0014(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(Instance[0014], attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0015(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(Instance[0015], attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0016(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(Instance[0016], attributeSyntaxNode?.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0017(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance[0017], node?.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0019(AttributeArgumentSyntax attributeArgumentNode, string typeParameterName)
        {
            return Diagnostic.Create(Instance[0019], attributeArgumentNode?.GetLocation(), typeParameterName);
        }
        public static Diagnostic CreateGA0020(AttributeArgumentSyntax attributeArgumentNode, IEnumerable<ITypeParameterSymbol> recursionPath)
        {
            return Diagnostic.Create(Instance[0020], attributeArgumentNode?.GetLocation(), string.Join(", ", recursionPath.Select(t => t.ToDisplayString())));
        }
        public static Diagnostic CreateGA0021(AttributeArgumentSyntax attributeArgumentNode)
        {
            return Diagnostic.Create(Instance[0021], attributeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0022(TypeParameterSyntax typeParameterDeclarationNode)
        {
            return Diagnostic.Create(Instance[0022], typeParameterDeclarationNode?.Identifier.GetLocation());
        }
        public static Diagnostic CreateGA0023(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, Instance[0023]);
        }
        public static Diagnostic CreateGA0024(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, Instance[0024]);
        }
        public static Diagnostic CreateGA0025(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, Instance[0025]);
        }
        public static Diagnostic CreateGA0025(BaseTypeSyntax baseTypeNode)
        {
            return Diagnostic.Create(Instance[0025], baseTypeNode?.GetLocation());
        }
        public static Diagnostic CreateGA0026(AttributeArgumentSyntax nonProfileTypeArgumentNode)
        {
            return Diagnostic.Create(Instance[0026], nonProfileTypeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0027(AttributeArgumentSyntax nonProfileGroupTypeArgumentNode)
        {
            return Diagnostic.Create(Instance[0027], nonProfileGroupTypeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0028(AttributeArgumentSyntax typeProfileInheritanceAttributeArgumentNode)
        {
            return Diagnostic.Create(Instance[0028], typeProfileInheritanceAttributeArgumentNode?.GetLocation());
        }
        public static Diagnostic CreateGA0029(InterfaceDeclarationSyntax interfaceDeclarationNode)
        {
            return CreateProfileInterfaceDiagnostic(interfaceDeclarationNode, Instance[0029]);
        }
        public static Diagnostic CreateGA0030(AttributeSyntax attributeNode)
        {
            return Diagnostic.Create(Instance[0030], attributeNode?.GetLocation());
        }

        private static Diagnostic CreateProfileInterfaceDiagnostic(InterfaceDeclarationSyntax interfaceDeclarationNode, DiagnosticDescriptor rule)
        {
            return Diagnostic.Create(rule, interfaceDeclarationNode?.Identifier.GetLocation());
        }
    }
}
