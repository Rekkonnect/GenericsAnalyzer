using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class AttributeArgumentSyntaxExtensions
    {
        public static void GetAttributeRelatedSyntaxNodes(this AttributeArgumentSyntax argument, out AttributeArgumentListSyntax argumentList, out AttributeSyntax attribute, out AttributeListSyntax attributeList)
        {
            argumentList = argument.Parent as AttributeArgumentListSyntax;
            attribute = argumentList.Parent as AttributeSyntax;
            attributeList = attribute.Parent as AttributeListSyntax;
        }
    }
}
