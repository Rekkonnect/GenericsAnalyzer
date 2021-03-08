using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ITypeParameterSymbolExtensions
    {
        public static ISymbol GetDeclaringSymbol(this ITypeParameterSymbol symbol)
        {
            return (ISymbol)symbol.DeclaringType ?? symbol.DeclaringMethod;
        }
    }
}
