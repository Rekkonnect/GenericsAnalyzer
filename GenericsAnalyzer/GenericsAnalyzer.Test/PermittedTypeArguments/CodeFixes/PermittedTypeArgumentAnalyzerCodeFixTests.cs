using GenericsAnalyzer.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using RoslynTestKit;
using System.Collections.Generic;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    public abstract class PermittedTypeArgumentAnalyzerCodeFixTests : BaseCodeFixDiagnosticTests
    {
        private static readonly MetadataReference[] metadataReferences = new[]
        {
            ReferenceSource.FromAssembly(typeof(TypeConstraintSystem).Assembly)
        };

        protected override IReadOnlyCollection<MetadataReference> References => metadataReferences;

        protected sealed override IReadOnlyCollection<DiagnosticAnalyzer> CreateAdditionalAnalyzers() => new DiagnosticAnalyzer[] { new PermittedTypeArgumentAnalyzer() };
    }
}
