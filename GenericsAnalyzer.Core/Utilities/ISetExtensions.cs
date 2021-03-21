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
        public static void RemoveRange<T>(this ISet<T> set, IEnumerable<T> elements)
        {
            foreach (var e in elements)
                set.Remove(e);
        }
        public static bool ContainsAll<T>(this ISet<T> set, IEnumerable<T> elements)
        {
            foreach (var e in elements)
                if (!set.Contains(e))
                    return false;
            return true;
        }    
    }
}
