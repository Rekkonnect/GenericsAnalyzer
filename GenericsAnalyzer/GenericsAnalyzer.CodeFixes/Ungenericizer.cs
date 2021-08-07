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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Ungenericizer))]
    public sealed class Ungenericizer : MultipleDiagnosticCodeFixProvider
    {
        protected override string CodeFixTitle => CodeFixResources.Ungenericizer_Title;

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new DiagnosticDescriptor[]
        {
            GA0023_Rule
        };

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var typeDeclarationNode = syntaxNode as TypeDeclarationSyntax;
            var decalarationIdentifier = typeDeclarationNode.Identifier;
            var triviaAfterTypeParameterList = typeDeclarationNode.TypeParameterList.GetTrailingTrivia();
            var identifierWithTrivia = decalarationIdentifier.WithTrailingTrivia(decalarationIdentifier.TrailingTrivia.AddRange(triviaAfterTypeParameterList));
            var appendedTriviaResultingNode = typeDeclarationNode.WithIdentifier(identifierWithTrivia);
            return await document.ReplaceNodeAsync(typeDeclarationNode, appendedTriviaResultingNode.WithTypeParameterList(null), cancellationToken);
        }
    }
}
