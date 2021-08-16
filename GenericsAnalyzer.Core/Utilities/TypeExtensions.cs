using System;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class TypeExtensions
    {
        public static Type GetGenericTypeDefinitionOrSame(this Type type)
        {
            if (!type.IsGenericType)
                return type;

            return type.GetGenericTypeDefinition();
        }
    }
}
