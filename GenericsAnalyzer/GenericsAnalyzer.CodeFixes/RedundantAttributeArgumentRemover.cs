using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeArgumentRemover))]
    public class RedundantAttributeArgumentRemover : MultipleDiagnosticCodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new[]
        {
            GA0003_Rule,
            GA0004_Rule,
            GA0005_Rule,
            GA0010_Rule,
            GA0011_Rule,
        };

        protected override string CodeFixTitle => CodeFixResources.RedundantAttributeArgumentRemover_Title;

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            return await context.RemoveAttributeArgumentAsync(syntaxNode as AttributeArgumentSyntax, SyntaxRemoveOptions.KeepNoTrivia, cancellationToken);
        }
    }
}
