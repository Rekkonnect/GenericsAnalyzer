using GenericsAnalyzer.Core.Utilities;
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

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.GetSyntaxRootAsync();

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

        protected abstract Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken);
    }
}
