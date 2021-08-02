using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class TypeKindExtensions
    {
        public static bool CanExplicitlyInheritTypes(this TypeKind kind)
        {
            switch (kind)
            {
                case TypeKind.Delegate:
                case TypeKind.Enum:
                    return false;
            }
            return true;
        }
    }
}
