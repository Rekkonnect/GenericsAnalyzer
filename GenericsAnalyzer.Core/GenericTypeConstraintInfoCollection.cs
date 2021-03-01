using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core
{
    public class GenericTypeConstraintInfoCollection
    {
        private Dictionary<ISymbol, GenericTypeConstraintInfo> dictionary = new Dictionary<ISymbol, GenericTypeConstraintInfo>(SymbolEqualityComparer.Default);

        public bool ContainsInfo(ISymbol symbol)
        {
            if (symbol is null)
                return false;

            return dictionary.ContainsKey(symbol);
        }

        public GenericTypeConstraintInfo this[ISymbol symbol]
        {
            get => dictionary[symbol];
            set
            {
                if (dictionary.ContainsKey(symbol))
                    dictionary[symbol] = value;
                else
                    dictionary.Add(symbol, value);
            }
        }
    }
}
