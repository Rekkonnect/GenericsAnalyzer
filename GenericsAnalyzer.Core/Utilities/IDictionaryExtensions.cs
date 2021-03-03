using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class IDictionaryExtensions
    {
        public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            else
                dict[key] = value;
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other)
        {
            foreach (var kvp in other)
                dict.Add(kvp.Key, kvp.Value);
        }
        public static void AddOrSetRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other)
        {
            foreach (var kvp in other)
                dict.AddOrSet(kvp.Key, kvp.Value);
        }
    }
}
