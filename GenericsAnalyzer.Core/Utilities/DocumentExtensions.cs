using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class DocumentExtensions
    {
        /// <summary>Gets the length difference between the new document and the original one.</summary>
        /// <param name="newDocument">The new document.</param>
        /// <param name="originalDocument">The original document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A positive value if the new document is larger than the original, a negative if the original is larger than the new one, or 0 if both have the same length. The documents' full span is taken into account when calculating the difference.</returns>
        public static async Task<int> LengthDifferenceFrom(this Document newDocument, Document originalDocument, CancellationToken cancellationToken = default)
        {
            var originalRootTask = originalDocument.GetSyntaxRootAsync(cancellationToken);
            var newRootTask = newDocument.GetSyntaxRootAsync(cancellationToken);

            var originalRoot = await originalRootTask;
            var newRoot = await newRootTask;

            return newRoot.FullSpan.Length - originalRoot.FullSpan.Length;
        }

        /// <summary>Removes the attribute that was specified. If the attribute's parent attribute list remains without any attributes, it is removed.</summary>
        /// <param name="document">The document whose attribute to remove.</param>
        /// <param name="attributeNode">The attribute node to remove from the document. It is mandatory that the attribute belong to the same document.</param>
        /// <param name="options">The options when performing the removal.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The resulting document that will not contain the removed argument.</returns>
        public static async Task<Document> RemoveAttributeCleanAsync
        (
            this Document document,
            AttributeSyntax attributeNode,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            SyntaxNode removedNode = attributeNode;
            if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = removedNode.Parent;

            return await RemoveSyntaxNodeAsync(document, removedNode, options, cancellationToken);
        }

        /// <summary>Removes the attribute argument that was specified. If the specified argument's parent attribute remains without any arguments, it is removed. If the attribute's parent attribute list remains without any attributes, it is removed.</summary>
        /// <param name="document">The document whose attribute argument to remove.</param>
        /// <param name="attributeArgumentNode">The attribute argument node to remove from the document. It is mandatory that the argument belong to the same document.</param>
        /// <param name="options">The options when performing the removal.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The resulting document that will not contain the removed argument.</returns>
        public static async Task<Document> RemoveAttributeArgumentCleanAsync
        (
            this Document document,
            AttributeArgumentSyntax attributeArgumentNode,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            SyntaxNode removedNode = attributeArgumentNode;

            if ((removedNode.Parent as AttributeArgumentListSyntax).Arguments.Count == 1)
            {
                removedNode = removedNode.Parent.Parent;
                if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                    removedNode = removedNode.Parent;
            }

            return await RemoveSyntaxNodeAsync(document, removedNode, options, cancellationToken);
        }

        /// <summary>Removes all attribute arguments that were specified. Any attribute that remains without any arguments is also removed. Any attribute list that remains without any attributes is also removed.</summary>
        /// <param name="document">The document whose attribute arguments to remove.</param>
        /// <param name="attributeArgumentNodes">The attribute argument nodes to remove from the document. It is mandatory that all arguments belong to the same document.</param>
        /// <param name="options">The options when performing the removal.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The resulting document that will not contain the removed arguments.</returns>
        /// <remarks>The operation has a complexity of O(n).</remarks>
        public static async Task<Document> RemoveAttributeArgumentsCleanAsync
        (
            this Document document,
            IEnumerable<AttributeArgumentSyntax> attributeArgumentNodes,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            var unprocessedArguments = new HashSet<AttributeArgumentSyntax>(attributeArgumentNodes);

            var removedArguments = new HashSet<AttributeArgumentSyntax>();
            var removedAttributes = new HashSet<AttributeSyntax>();
            var removedAttributeLists = new HashSet<AttributeListSyntax>();

            var locallyProcessedArguments = new HashSet<AttributeArgumentSyntax>();

            while (unprocessedArguments.Any())
            {
                var argument = unprocessedArguments.First();
                argument.GetAttributeRelatedSyntaxNodes(out var argumentList, out var attribute, out var attributeList);

                foreach (var arg in argumentList.Arguments)
                {
                    if (!unprocessedArguments.Remove(arg))
                        continue;

                    locallyProcessedArguments.Add(arg);
                }

                if (locallyProcessedArguments.Count == argumentList.Arguments.Count)
                {
                    // All attribute arguments were requested for deletion, therefore we should at least remove the entire attribute
                    removedAttributes.Add(attribute);

                    bool containsAll = removedAttributes.ContainsAll(attributeList.Attributes);
                    if (containsAll)
                    {
                        removedAttributes.RemoveRange(attributeList.Attributes);
                        removedAttributeLists.Add(attributeList);
                    }
                }
                else
                {
                    removedArguments.AddRange(locallyProcessedArguments);
                }

                locallyProcessedArguments.Clear();
            }

            var removedNodes = removedArguments.AsEnumerable<SyntaxNode>().Concat(removedAttributes).Concat(removedAttributeLists);

            return document = await RemoveSyntaxNodesAsync(document, removedNodes, options, cancellationToken);
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

        public static async Task<Document> RemoveSyntaxNodesAsync
        (
            this Document document,
            IEnumerable<SyntaxNode> removedNodes,
            SyntaxRemoveOptions options = SyntaxRemoveOptions.KeepExteriorTrivia,
            CancellationToken cancellationToken = default
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.RemoveNodes(removedNodes, options));
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
