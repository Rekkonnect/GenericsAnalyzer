using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeRemover))]
    public class RedundantAttributeRemover : CodeFixProvider
    {
        private ImmutableArray<string> fixableDiagnosticIds = new[]
        {
            GA0014_Rule.Id,
            GA0015_Rule.Id,
            GA0016_Rule.Id,
            GA0018_Rule.Id,
        }.ToImmutableArray();

        public sealed override ImmutableArray<string> FixableDiagnosticIds => fixableDiagnosticIds;

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostics = context.Diagnostics.Where(d => fixableDiagnosticIds.Contains(d.Id));

            foreach (var diagnostic in diagnostics)
            {
                var attributeSyntax = root.FindNode(diagnostic.Location.SourceSpan) as AttributeSyntax;

                var codeAction = CodeAction.Create(CodeFixResources.RedundantAttributeRemover_Title, PerformAction, nameof(RedundantAttributeRemover));
                context.RegisterCodeFix(codeAction, diagnostic);

                Task<Document> PerformAction(CancellationToken token)
                {
                    return RemoveAttributeAsync(context, attributeSyntax, token);
                }
            }
        }

        private async Task<Document> RemoveAttributeAsync(CodeFixContext context, AttributeSyntax attributeSyntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            SyntaxNode removedNode = attributeSyntaxNode;
            if ((attributeSyntaxNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = attributeSyntaxNode.Parent;

            return document.WithSyntaxRoot(root.RemoveNode(removedNode, SyntaxRemoveOptions.KeepNoTrivia));
        }
    }
}
