using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ITypeSymbolExtensions
    {
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

        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this ITypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.AllInterfaces)
                yield return baseInterface;
        }
        public static IEnumerable<INamedTypeSymbol> GetAllDirectBaseTypes(this ITypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.Interfaces)
                yield return baseInterface;
        }
    }
}
