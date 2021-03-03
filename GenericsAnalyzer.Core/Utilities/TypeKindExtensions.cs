using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class TypeKindExtensions
    {
        public static bool CanInheritTypes(this TypeKind kind)
        {
            return kind != TypeKind.Delegate;
        }
    }
}
