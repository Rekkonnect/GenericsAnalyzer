using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GenericsAnalyzer
{
    public abstract class MultipleDiagnosticCodeFixProvider : CodeFixProvider
    {
        private ImmutableArray<string> fixableDiagnosticIds;

        protected abstract IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors { get; }
        public sealed override ImmutableArray<string> FixableDiagnosticIds => fixableDiagnosticIds;

        protected abstract string CodeFixTitle { get; }

        protected MultipleDiagnosticCodeFixProvider()
        {
            fixableDiagnosticIds = FixableDiagnosticDescriptors.Select(d => d.Id).ToImmutableArray();
        }

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await GetSyntaxRootAsync(context);

            foreach (var diagnostic in context.Diagnostics)
            {
                var codeAction = CodeAction.Create(CodeFixTitle, PerformAction, GetType().Name);
                context.RegisterCodeFix(codeAction, diagnostic);

                Task<Document> PerformAction(CancellationToken token)
                {
                    var syntaxNode = root.FindNode(diagnostic.Location.SourceSpan);
                    return PerformCodeFixActionAsync(context, syntaxNode, token);
                }
            }
        }

        #region Document
        // TODO: Migrate as extensions to not require relying on this class
        // Inconsistent signatures detected
        protected async Task<Document> RemoveAttributeAsync(Document document, AttributeSyntax attributeSyntax, CancellationToken cancellationToken)
        {
            SyntaxNode removedNode = attributeSyntax;
            if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = removedNode.Parent;

            return await RemoveSyntaxNodeAsync(document, cancellationToken, removedNode);
        }
        protected async Task<Document> RemoveAttributeArgumentAsync(Document document, AttributeArgumentSyntax attributeArgumentSyntax, CancellationToken cancellationToken)
        {
            SyntaxNode removedNode = attributeArgumentSyntax;

            if ((removedNode.Parent as AttributeArgumentListSyntax).Arguments.Count == 1)
            {
                removedNode = removedNode.Parent.Parent;
                if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                    removedNode = removedNode.Parent;
            }

            return await RemoveSyntaxNodeAsync(document, cancellationToken, removedNode);
        }

        protected async Task<Document> RemoveSyntaxNodeAsync(Document document, CancellationToken cancellationToken, SyntaxNode removedNode)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.RemoveNode(removedNode, SyntaxRemoveOptions.KeepNoTrivia));
        }
        protected async Task<Document> InsertSyntaxNodesAfterAsync(Document document, CancellationToken cancellationToken, SyntaxNode referenceNode, IEnumerable<SyntaxNode> insertedNodes)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.InsertNodesAfter(referenceNode, insertedNodes));
        }
        protected async Task<Document> InsertSyntaxNodesBeforeAsync(Document document, CancellationToken cancellationToken, SyntaxNode referenceNode, IEnumerable<SyntaxNode> insertedNodes)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.InsertNodesBefore(referenceNode, insertedNodes));
        }
        protected async Task<Document> ReplaceNodeAsync(Document document, CancellationToken cancellationToken, SyntaxNode oldNode, IEnumerable<SyntaxNode> insertedNodes)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.ReplaceNode(oldNode, insertedNodes));
        }

        protected async Task<Document> InsertSyntaxNodesAfterAsync(Document document, CancellationToken cancellationToken, SyntaxNode referenceNode, params SyntaxNode[] insertedNodes)
            => await InsertSyntaxNodesAfterAsync(document, cancellationToken, referenceNode, (IEnumerable<SyntaxNode>)insertedNodes);
        protected async Task<Document> InsertSyntaxNodesBeforeAsync(Document document, CancellationToken cancellationToken, SyntaxNode referenceNode, params SyntaxNode[] insertedNodes)
            => await InsertSyntaxNodesBeforeAsync(document, cancellationToken, referenceNode, (IEnumerable<SyntaxNode>)insertedNodes);
        protected async Task<Document> ReplaceNodeAsync(Document document, CancellationToken cancellationToken, SyntaxNode oldNode, params SyntaxNode[] insertedNodes)
            => await ReplaceNodeAsync(document, cancellationToken, oldNode, (IEnumerable<SyntaxNode>)insertedNodes);
        #endregion

        #region Context
        protected async Task<Document> RemoveAttributeAsync(CodeFixContext context, AttributeSyntax attributeSyntax, CancellationToken cancellationToken)
            => await RemoveAttributeAsync(context.Document, attributeSyntax, cancellationToken);
        protected async Task<Document> RemoveAttributeArgumentAsync(CodeFixContext context, AttributeArgumentSyntax attributeArgumentSyntax, CancellationToken cancellationToken)
            => await RemoveAttributeArgumentAsync(context.Document, attributeArgumentSyntax, cancellationToken);

        protected async Task<Document> RemoveSyntaxNodeAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode removedNode)
            => await RemoveSyntaxNodeAsync(context.Document, cancellationToken, removedNode);
        protected async Task<Document> InsertSyntaxNodesAfterAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode referenceNode, IEnumerable<SyntaxNode> insertedNodes)
            => await InsertSyntaxNodesAfterAsync(context.Document, cancellationToken, referenceNode, insertedNodes);
        protected async Task<Document> InsertSyntaxNodesBeforeAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode referenceNode, IEnumerable<SyntaxNode> insertedNodes)
            => await InsertSyntaxNodesBeforeAsync(context.Document, cancellationToken, referenceNode, insertedNodes);

        protected async Task<Document> InsertSyntaxNodesAfterAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode referenceNode, params SyntaxNode[] insertedNodes)
            => await InsertSyntaxNodesAfterAsync(context, cancellationToken, referenceNode, (IEnumerable<SyntaxNode>)insertedNodes);
        protected async Task<Document> InsertSyntaxNodesBeforeAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode referenceNode, params SyntaxNode[] insertedNodes)
            => await InsertSyntaxNodesBeforeAsync(context, cancellationToken, referenceNode, (IEnumerable<SyntaxNode>)insertedNodes);
        #endregion

        protected async Task<SyntaxNode> GetSyntaxRootAsync(CodeFixContext context) => await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        protected abstract Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken);
    }
}
