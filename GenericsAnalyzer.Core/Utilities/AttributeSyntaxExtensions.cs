using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class AttributeSyntaxExtensions
    {
        public static string GetAttributeIdentifierString(this AttributeSyntax attribute) => (attribute.Name as IdentifierNameSyntax).Identifier.ValueText;

        /// <summary>Determines whether the given attribute is a generic constaint attribute that the analyzer should take into account.</summary>
        /// <param name="attribute">The attribute that will be evaluated.</param>
        /// <returns><see langword="true"/> if the given attribute is a generic constraint attribute one that is important enough, otherwise <see langword="false"/>.</returns>
        public static bool IsGenericConstraintAttribute(this AttributeSyntax attribute, SemanticModel semanticModel)
        {
            return IsGenericConstraintAttribute<IGenericTypeConstraintAttribute>(attribute, semanticModel);
        }
        /// <summary>Determines whether the given attribute is a generic constaint attribute that the analyzer should take into account.</summary>
        /// <typeparam name="T">The base type that the attribute should inherit if it's an important one.</typeparam>
        /// <param name="attribute">The attribute that will be evaluated.</param>
        /// <returns><see langword="true"/> if the given attribute is a generic constraint attribute one that is important enough, otherwise <see langword="false"/>.</returns>
        public static bool IsGenericConstraintAttribute<T>(this AttributeSyntax attribute, SemanticModel semanticModel)
            where T : IGenericTypeConstraintAttribute
        {
            return semanticModel.GetTypeInfo(attribute).Type.GetAllBaseTypesAndInterfaces().Any(t => t.Name == typeof(T).Name);
        }
    }
}
