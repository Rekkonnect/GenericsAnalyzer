using GenericsAnalyzer.Core;
using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DuplicateAttributeArgumentRemover))]
    public class DuplicateAttributeArgumentRemover : MultipleDiagnosticCodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new[]
        {
            GA0002_Rule,
            GA0009_Rule,
        };

        protected override string CodeFixTitle => CodeFixResources.DuplicateAttributeArgumentRemover_Title;

        public override FixAllProvider GetFixAllProvider() => null;

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            var argument = syntaxNode as AttributeArgumentSyntax;
            var targetType = GetTypeSymbol(argument);

            argument.GetAttributeRelatedSyntaxNodes(out var argumentList, out var attribute, out var attributeList);

            var typeParameter = attributeList.Parent as TypeParameterSyntax;
            var attributes = typeParameter.AttributeLists.SelectMany(l => l.Attributes)
                .Where(a => a.ArgumentList?.Arguments.Count > 0)
                .Where(a => a.IsGenericConstraintAttribute<ConstrainedTypesAttributeBase>(semanticModel));

            var arguments = attributes.SelectMany(a => a.ArgumentList.Arguments);
            arguments = arguments.Where(a => a != argument);
            var removed = arguments.Where(ArgumentRemovalPredicate);

            return await context.RemoveAttributeArgumentsCleanAsync(removed, SyntaxRemoveOptions.KeepNoTrivia, cancellationToken);

            ITypeSymbol GetTypeSymbol(AttributeArgumentSyntax a)
            {
                return semanticModel.GetTypeInfo((a.Expression as TypeOfExpressionSyntax)?.Type).Type;
            }
            bool ArgumentRemovalPredicate(AttributeArgumentSyntax arg)
            {
                // DEBUG
                var typeSymbol = GetTypeSymbol(arg);
                return typeSymbol.Equals(targetType, SymbolEqualityComparer.Default);
                // END DEBUG
                return GetTypeSymbol(arg).Equals(targetType, SymbolEqualityComparer.Default);
            }
        }
    }
}
