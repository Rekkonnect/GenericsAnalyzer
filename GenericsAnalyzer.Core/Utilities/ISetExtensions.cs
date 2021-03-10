using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ISetExtensions
    {
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> elements)
        {
            foreach (var e in elements)
                set.Add(e);
        }
    }
}
