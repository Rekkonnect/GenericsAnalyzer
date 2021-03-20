using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class SyntaxNodeExtensions
    {
        public static T GetParentRecursively<T>(this SyntaxNode node)
            where T : SyntaxNode
        {
            var parent = node;
            T matchingParent;
            while ((matchingParent = parent as T) is null)
                parent = parent.Parent;
            return matchingParent;
        }
    }
}
