using Microsoft.CodeAnalysis;
using System;

namespace GenericsAnalyzer.Core
{
    public static class TypeSymbolHelpers
    {
        public static bool EqualsType(ITypeSymbol symbol, Type type)
        {
            return type.FullName == symbol.MetadataName
                && type.Assembly.FullName == symbol.ContainingAssembly.MetadataName;
        }
    }
}
