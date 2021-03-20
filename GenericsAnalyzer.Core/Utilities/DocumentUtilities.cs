using Microsoft.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class DocumentUtilities
    {
        public static async Task<int> GetLengthDifference(Document originalDocument, Document newDocument, CancellationToken cancellationToken = default)
        {
            var originalRootTask = originalDocument.GetSyntaxRootAsync(cancellationToken);
            var newRootTask = newDocument.GetSyntaxRootAsync(cancellationToken);

            var originalRoot = await originalRootTask;
            var newRoot = await newRootTask;

            return newRoot.FullSpan.Length - originalRoot.FullSpan.Length;
        }
    }
}
