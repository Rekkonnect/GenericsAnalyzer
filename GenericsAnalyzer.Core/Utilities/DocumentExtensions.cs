using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class DocumentExtensions
    {
        public static async Task<int> LengthDifferenceFrom(this Document newDocument, Document originalDocument, CancellationToken cancellationToken = default)
        {
            var originalRootTask = originalDocument.GetSyntaxRootAsync(cancellationToken);
            var newRootTask = newDocument.GetSyntaxRootAsync(cancellationToken);

            var originalRoot = await originalRootTask;
            var newRoot = await newRootTask;

            return newRoot.FullSpan.Length - originalRoot.FullSpan.Length;
        }

        public static async Task<Document> RemoveAttributeAsync
        (
            this Document document,
            AttributeSyntax attributeSyntax,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            SyntaxNode removedNode = attributeSyntax;
            if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = removedNode.Parent;

            return await RemoveSyntaxNodeAsync(document, removedNode, options, cancellationToken);
        }

        public static async Task<Document> RemoveAttributeArgumentAsync
        (
            this Document document,
            AttributeArgumentSyntax attributeArgumentSyntax,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            SyntaxNode removedNode = attributeArgumentSyntax;

            if ((removedNode.Parent as AttributeArgumentListSyntax).Arguments.Count == 1)
            {
                removedNode = removedNode.Parent.Parent;
                if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                    removedNode = removedNode.Parent;
            }

            return await RemoveSyntaxNodeAsync(document, removedNode, options, cancellationToken);
        }

        public static async Task<Document> RemoveSyntaxNodeAsync
        (
            this Document document,
            SyntaxNode removedNode,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.RemoveNode(removedNode, options));
        }

        public static async Task<Document> InsertSyntaxNodesAfterAsync
        (
            this Document document,
            SyntaxNode referenceNode,
            IEnumerable<SyntaxNode> insertedNodes,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.InsertNodesAfter(referenceNode, insertedNodes));
        }

        public static async Task<Document> InsertSyntaxNodesBeforeAsync
        (
            this Document document,
            SyntaxNode referenceNode,
            IEnumerable<SyntaxNode> insertedNodes,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.InsertNodesBefore(referenceNode, insertedNodes));
        }

        public static async Task<Document> ReplaceNodeAsync
        (
            this Document document,
            SyntaxNode oldNode,
            IEnumerable<SyntaxNode> insertedNodes,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.ReplaceNode(oldNode, insertedNodes));
        }

        public static async Task<Document> ReplaceNodeAsync
        (
            this Document document,
            SyntaxNode oldNode,
            SyntaxNode insertedNode,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.ReplaceNode(oldNode, insertedNode));
        }
    }
}
