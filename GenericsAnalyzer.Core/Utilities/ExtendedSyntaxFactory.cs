using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ExtendedSyntaxFactory
    {
        public static AttributeSyntax Attribute<T>(bool assumeImported = true)
            where T : Attribute
        {
            return Attribute<T>(null, assumeImported);
        }
        public static AttributeSyntax Attribute(Type attributeType, bool assumeImported = true)
        {
            return Attribute(attributeType, null, assumeImported);
        }
        public static AttributeSyntax Attribute<T>(AttributeArgumentListSyntax attributeArgumentList, bool assumeImported = true)
            where T : Attribute
        {
            return Attribute(typeof(T), attributeArgumentList, assumeImported);
        }
        public static AttributeSyntax Attribute(Type attributeType, AttributeArgumentListSyntax attributeArgumentList, bool assumeImported = true)
        {
            var attributeName = SimplifyAttributeNameUsage(assumeImported ? attributeType.Name : attributeType.FullName);
            return SyntaxFactory.Attribute(SyntaxFactory.ParseName(attributeName), attributeArgumentList);
        }

        public static AttributeListSyntax AttributeList(AttributeSyntax attributeSyntax)
        {
            return SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[] { attributeSyntax }));
        }

        public static void SimplifyAttributeNameUsage(ref string attributeTypeName)
        {
            attributeTypeName = SimplifyAttributeNameUsage(attributeTypeName);
        }
        public static string SimplifyAttributeNameUsage(string attributeTypeName)
        {
            const string attributeSuffix = nameof(System.Attribute);

            if (attributeTypeName.EndsWith(attributeSuffix))
                return attributeTypeName.Remove(attributeTypeName.Length - attributeSuffix.Length);

            return attributeTypeName;
        }
    }
}
