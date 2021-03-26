using System;
using System.Linq;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class EnumHelpers
    {
        public static T[] GetValues<T>()
            where T : struct, Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
