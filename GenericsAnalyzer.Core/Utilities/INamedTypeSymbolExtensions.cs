using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class INamedTypeSymbolExtensions
    {
        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.AllInterfaces)
                yield return baseInterface;
        }
        public static IEnumerable<INamedTypeSymbol> GetAllDirectBaseTypes(this INamedTypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            if (baseType != null)
                yield return baseType;

            foreach (var baseInterface in symbol.Interfaces)
                yield return baseInterface;
        }
    }
}
