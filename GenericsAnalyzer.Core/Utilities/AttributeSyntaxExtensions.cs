using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class AttributeSyntaxExtensions
    {
        public static string GetAttributeIdentifierString(this AttributeSyntax attribute) => (attribute.Name as IdentifierNameSyntax).Identifier.ValueText;
    }
}
