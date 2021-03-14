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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RedundantAttributeRemover))]
    public class RedundantAttributeRemover : MultipleDiagnosticCodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new[]
        {
            GA0014_Rule,
            GA0015_Rule,
            GA0016_Rule,
            GA0018_Rule,
        };

        protected override string CodeFixTitle => CodeFixResources.RedundantAttributeRemover_Title;

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            SyntaxNode removedNode = syntaxNode;
            if ((removedNode.Parent as AttributeListSyntax).Attributes.Count == 1)
                removedNode = removedNode.Parent;

            return await RemoveSyntaxNodeAsync(context, cancellationToken, removedNode);
        }
    }
}
