﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoseLynn;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.GADiagnosticDescriptorStorage;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Ungenericizer))]
    public sealed class Ungenericizer : GACodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new DiagnosticDescriptor[]
        {
            Instance[0023]
        };

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var semanticModel = await document.GetSemanticModelAsync();
            var typeDeclarationNode = syntaxNode as TypeDeclarationSyntax;
            var declaredSymbol = semanticModel.GetDeclaredSymbol(typeDeclarationNode);
            var declaringNodes = declaredSymbol.DeclaringSyntaxReferences.Select(reference => reference.GetSyntax(cancellationToken) as MemberDeclarationSyntax);
            return await document.RemoveTypeParameterListsAsync(declaringNodes, cancellationToken);
        }
    }
}
