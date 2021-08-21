﻿using GenericsAnalyzer.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoseLynn;
using RoseLynn.CSharp.Syntax;
using RoseLynn.Utilities;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GenericsAnalyzer.GADiagnosticDescriptorStorage;

namespace GenericsAnalyzer
{
    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ProfileInterfaceDeclarer))]
    public sealed class ProfileInterfaceDeclarer : MultipleDiagnosticCodeFixProvider
    {
        protected override IEnumerable<DiagnosticDescriptor> FixableDiagnosticDescriptors => new DiagnosticDescriptor[]
        {
            Instance.GA0030_Rule
        };

        protected override async Task<Document> PerformCodeFixActionAsync(CodeFixContext context, SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var semanticModel = await document.GetSemanticModelAsync();
            var interfaceDeclarationNode = syntaxNode.GetNearestParentOfType<InterfaceDeclarationSyntax>();
            var profileAttribute = ExtendedSyntaxFactory.Attribute<TypeConstraintProfileAttribute>();

            var profileGroupAttribute = interfaceDeclarationNode.AttributeLists
                .SelectMany(list => list.Attributes)
                .Where(attribute => semanticModel.GetTypeInfo(attribute).Type.Name == nameof(TypeConstraintProfileGroupAttribute))
                .OnlyOrDefault();

            if (profileGroupAttribute is null)
            {
                // Create new profile attribute
                var newInterfaceDeclarationNode = interfaceDeclarationNode.AddAttributeLists(ExtendedSyntaxFactory.AttributeList(profileAttribute));
                return await document.ReplaceNodeAsync(interfaceDeclarationNode, newInterfaceDeclarationNode);
            }
            else
            {
                // Replace existing profile group attribute with the created profile one
                return await document.ReplaceNodeAsync(profileGroupAttribute, profileAttribute);
            }
        }
    }
}
