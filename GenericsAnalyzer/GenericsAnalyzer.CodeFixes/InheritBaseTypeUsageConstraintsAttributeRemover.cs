using GenericsAnalyzer.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InheritBaseTypeUsageConstraintsAttributeRemover)), Shared]
    public class InheritBaseTypeUsageConstraintsAttributeRemover : CodeFixProvider
    {
        private ImmutableArray<string> fixableDiagnosticIds = new[]
        {
            GA0014_Rule.Id,
            GA0015_Rule.Id,
            GA0016_Rule.Id,
        }.ToImmutableArray();

        public sealed override ImmutableArray<string> FixableDiagnosticIds => fixableDiagnosticIds;

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostics = context.Diagnostics.Where(d => fixableDiagnosticIds.Contains(d.Id));

            foreach (var diagnostic in diagnostics)
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;

                var attributeSyntax = root.FindNode(diagnosticSpan) as AttributeSyntax;

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        CodeFixResources.RedundantAttributeRemover_Title,
                        c => RemoveAttributeAsync(context, attributeSyntax, c),
                        nameof(CodeFixResources.RedundantAttributeRemover_Title)),
                    diagnostic);
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
