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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeArgumentRemover))]
    public class RedundantAttributeArgumentRemover : GACodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new[]
        {
            Instance.GA0003_Rule,
            Instance.GA0004_Rule,
            Instance.GA0005_Rule,
            Instance.GA0010_Rule,
            Instance.GA0011_Rule,
        };

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            return await context.RemoveAttributeArgumentCleanAsync(syntaxNode as AttributeArgumentSyntax, SyntaxRemoveOptions.KeepNoTrivia, cancellationToken);
        }
    }
}
