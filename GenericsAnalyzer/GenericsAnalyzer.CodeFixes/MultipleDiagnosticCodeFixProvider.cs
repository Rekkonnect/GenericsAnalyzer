using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
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

        protected async Task<Document> RemoveSyntaxNodeAsync(CodeFixContext context, CancellationToken cancellationToken, SyntaxNode removedNode)
        {
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(root.RemoveNode(removedNode, SyntaxRemoveOptions.KeepNoTrivia));
        }

        protected async Task<SyntaxNode> GetSyntaxRootAsync(CodeFixContext context) => await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        protected abstract Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken);
    }
}
