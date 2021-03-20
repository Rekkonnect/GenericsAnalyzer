using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class IEnumerableExtensions
    {
        /// <summary>Gets the only element of the sequence, if it only has one element, otherwise returns <see langword="default"/>.</summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The only element of the sequence, or <see langword="default"/>.</returns>
        public static T OnlyOrDefault<T>(this IEnumerable<T> source)
        {
            if (source is null)
                return default;

            T first = default;
            bool hasElements = false;

            foreach (var s in source)
            {
                if (hasElements)
                    return default;

                first = s;
                hasElements = true;
            }

            return first;
        }
    }
}
