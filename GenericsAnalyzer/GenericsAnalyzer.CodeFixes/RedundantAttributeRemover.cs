using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoseLynn.CodeFixes;
using System.Collections.Generic;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.GADiagnosticDescriptorStorage;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeRemover))]
    public class RedundantAttributeRemover : MultipleDiagnosticCodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new[]
        {
            Instance.GA0014_Rule,
            Instance.GA0015_Rule,
            Instance.GA0016_Rule,
        };

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            return await context.RemoveAttributeCleanAsync(syntaxNode as AttributeSyntax, SyntaxRemoveOptions.KeepNoTrivia, cancellationToken);
        }
    }
}
