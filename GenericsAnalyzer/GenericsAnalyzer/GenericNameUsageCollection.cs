using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace GenericsAnalyzer
{
    public class GenericNameUsageCollection
    {
        private Dictionary<ISymbol, List<GenericNameSyntax>> usages = new Dictionary<ISymbol, List<GenericNameSyntax>>(SymbolEqualityComparer.Default);

        public void Register(ISymbol name, GenericNameSyntax node)
        {
            if (!usages.ContainsKey(name))
                usages.Add(name, new List<GenericNameSyntax>());
            
            usages[name].Add(node);
        }

        public List<GenericNameSyntax> this[ISymbol name] => new List<GenericNameSyntax>(usages[name]);
    }
}
