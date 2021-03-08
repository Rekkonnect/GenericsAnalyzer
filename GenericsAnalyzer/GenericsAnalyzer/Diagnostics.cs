using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    internal static class Diagnostics
    {
        public static Diagnostic CreateGA0001(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0001_Rule, node.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateGA0014(AttributeSyntax attributeSyntaxNode, ISymbol symbol)
        {
            return Diagnostic.Create(GA0014_Rule, attributeSyntaxNode.GetLocation(), symbol.ToDisplayString());
        }
        public static Diagnostic CreateGA0017(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(GA0017_Rule, node.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
    }
}
