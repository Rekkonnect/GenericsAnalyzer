using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class INamedTypeSymbolExtensions
    {
        public static bool IsUnboundGenericTypeSafe(this INamedTypeSymbol symbol)
        {
            return symbol.IsGenericType && symbol.IsUnboundGenericType;
        }
        public static bool IsBoundGenericTypeSafe(this INamedTypeSymbol symbol)
        {
            return symbol.IsGenericType && !symbol.IsUnboundGenericType;
        }
    }
}
