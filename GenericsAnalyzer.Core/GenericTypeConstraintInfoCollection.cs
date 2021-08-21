using Microsoft.CodeAnalysis;
using RoseLynn.Utilities;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core
{
    public class GenericTypeConstraintInfoCollection
    {
        private Dictionary<ISymbol, GenericTypeConstraintInfo> finalizedSymbols = new(SymbolEqualityComparer.Default);
        private Dictionary<ISymbol, GenericTypeConstraintInfo.Builder> builders = new(SymbolEqualityComparer.Default);

        public bool ContainsInfo(ISymbol symbol)
        {
            if (symbol is null)
                return false;

            return finalizedSymbols.ContainsKey(symbol) || builders.ContainsKey(symbol);
        }

        public GenericTypeConstraintInfo.Builder GetBuilder(ISymbol symbol)
        {
            return builders[symbol];
        }
        public void SetBuilder(ISymbol symbol, GenericTypeConstraintInfo.Builder builder)
        {
            // Avoid creating a builder if a finalized generic name exists
            if (finalizedSymbols.ContainsKey(symbol))
                return;

            builders.AddOrSet(symbol, builder);
        }

        public void GetBuilderOrFinalizedInfo(ISymbol symbol, out GenericTypeConstraintInfo finalizedInfo, out GenericTypeConstraintInfo.Builder builder)
        {
            builder = null;

            if (finalizedSymbols.TryGetValue(symbol, out finalizedInfo))
                return;

            builders.TryGetValue(symbol, out builder);
        }

        public void FinalizeGenericSymbol(ISymbol symbol)
        {
            if (!builders.ContainsKey(symbol))
                return;

            var builder = builders[symbol];
            var result = builder.FinalizeTypeInfo();
            builders.Remove(symbol);
            // Finalized symbols cannot contain unassigned 
            finalizedSymbols.Add(symbol, result);
        }

        public GenericTypeConstraintInfo this[ISymbol symbol]
        {
            get => finalizedSymbols[symbol];
            set
            {
                // Safely discard the builder that was assigned for the symbols
                if (value != null)
                    builders.Remove(symbol);

                finalizedSymbols.AddOrSet(symbol, value);
            }
        }
    }
}
