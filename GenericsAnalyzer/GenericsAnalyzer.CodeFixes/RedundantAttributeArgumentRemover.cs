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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeArgumentRemover))]
    public class RedundantAttributeArgumentRemover : CodeFixProvider
    {
        private ImmutableArray<string> fixableDiagnosticIds = new[]
        {
            GA0010_Rule.Id,
            GA0011_Rule.Id,
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
                var attributeSyntax = root.FindNode(diagnostic.Location.SourceSpan) as AttributeArgumentSyntax;

                var codeAction = CodeAction.Create(CodeFixResources.RedundantAttributeArgumentRemover_Title, PerformAction, nameof(RedundantAttributeArgumentRemover));
                context.RegisterCodeFix(codeAction, diagnostic);

                Task<Document> PerformAction(CancellationToken token)
                {
                    return RemoveAttributeArgumentAsync(context, attributeSyntax, token);
                }
            }
        }

        private async Task<Document> RemoveAttributeArgumentAsync(CodeFixContext context, AttributeArgumentSyntax attributeArgumentSyntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            SyntaxNode removedNode = attributeArgumentSyntaxNode;

            if ((removedNode.Parent as AttributeArgumentListSyntax).Arguments.Count == 1)
                removedNode = removedNode.Parent.Parent;
            if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = removedNode.Parent;

            return document.WithSyntaxRoot(root.RemoveNode(removedNode, SyntaxRemoveOptions.KeepNoTrivia));
        }
    }
}
