using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class SyntaxNodeParentRetrievalExtensions
    {
        public static void GetAttributeRelatedParents(this AttributeArgumentSyntax argument, out AttributeArgumentListSyntax argumentList, out AttributeSyntax attribute, out AttributeListSyntax attributeList)
        {
            argumentList = argument.GetParentAttributeArgumentList();
            attribute = argumentList.GetParentAttribute();
            attributeList = attribute.GetParentAttributeList();
        }

        public static AttributeArgumentListSyntax GetParentAttributeArgumentList(this AttributeArgumentSyntax argument)
        {
            return argument.Parent as AttributeArgumentListSyntax;
        }
        public static AttributeSyntax GetParentAttribute(this AttributeArgumentListSyntax argumentList)
        {
            return argumentList.Parent as AttributeSyntax;
        }
        public static AttributeListSyntax GetParentAttributeList(this AttributeSyntax attribute)
        {
            return attribute.Parent as AttributeListSyntax;
        }
    }
}
