using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ITypeSymbolExtensions
    {
        /// <summary>Determines whether the given attribute type is a generic constaint attribute that the analyzer should take into account.</summary>
        /// <param name="attributeType">The attribute type that will be evaluated.</param>
        /// <returns><see langword="true"/> if the given attribute type is a generic constraint attribute one that is important enough, otherwise <see langword="false"/>.</returns>
        public static bool IsGenericConstraintAttribute(this ITypeSymbol attributeType)
        {
            return IsGenericConstraintAttribute<IGenericTypeConstraintAttribute>(attributeType);
        }
        /// <summary>Determines whether the given attribute type is a generic constaint attribute that the analyzer should take into account.</summary>
        /// <typeparam name="T">The base type that the attribute type should inherit if it's an important one.</typeparam>
        /// <param name="attributeType">The attribute type that will be evaluated.</param>
        /// <returns><see langword="true"/> if the given attribute type is a generic constraint attribute one that is important enough, otherwise <see langword="false"/>.</returns>
        public static bool IsGenericConstraintAttribute<T>(this ITypeSymbol attributeType)
            where T : IGenericTypeConstraintAttribute
        {
            return attributeType.GetAllBaseTypesAndInterfaces().Any(t => t.Name == typeof(T).Name);
        }

        public static bool IsValidTypeArgument(this ITypeSymbol symbol)
        {
            return !IsInvalidTypeArgument(symbol);
        }
        public static bool IsInvalidTypeArgument(this ITypeSymbol symbol)
        {
            return symbol is IPointerTypeSymbol
                || symbol.SpecialType is SpecialType.System_Void;
        }

        public static bool IsNotNull(this ITypeSymbol symbol)
        {
            if (symbol.IsValueType)
                return true;

            return symbol.NullableAnnotation == NullableAnnotation.Annotated;
        }

        public static bool Inherits(this ITypeSymbol symbol, INamedTypeSymbol other)
        {
            if (other.TypeKind == TypeKind.Interface)
                return symbol.AllInterfaces.Contains(other);

            return symbol.GetAllBaseTypes().Contains(other, SymbolEqualityComparer.Default);
        }

        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this ITypeSymbol symbol)
        {
            var currentType = symbol;
            while (currentType != null)
            {
                var baseType = currentType.BaseType;
                if (baseType != null)
                    yield return baseType;
                currentType = baseType;
            }
        }
        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypesAndInterfaces(this ITypeSymbol symbol)
        {
            return GetAllBaseTypes(symbol).Concat(symbol.AllInterfaces);
        }
        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypesAndDirectInterfaces(this ITypeSymbol symbol)
        {
            return GetAllBaseTypes(symbol).Concat(symbol.Interfaces);
        }
        public static IEnumerable<INamedTypeSymbol> GetBaseTypeAndInterfaces(this ITypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.AllInterfaces)
                yield return baseInterface;
        }
        public static IEnumerable<INamedTypeSymbol> GetBaseTypeAndDirectInterfaces(this ITypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.Interfaces)
                yield return baseInterface;
        }
    }
}
