using Microsoft.CodeAnalysis;

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
    }
}
