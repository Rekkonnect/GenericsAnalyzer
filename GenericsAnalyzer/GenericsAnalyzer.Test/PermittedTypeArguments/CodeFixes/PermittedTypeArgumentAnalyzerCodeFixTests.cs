using Microsoft.CodeAnalysis.CodeFixes;
using static GenericsAnalyzer.Test.Helpers.UsingsHelpers;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    public abstract class PermittedTypeArgumentAnalyzerCodeFixTests<TCodeFix> : BaseCodeFixDiagnosticTests<PermittedTypeArgumentAnalyzer, TCodeFix>
        where TCodeFix : CodeFixProvider, new()
    {
        public void TestCodeFixWithUsings(string markupCode, string expected) => TestCodeFix(WithUsings(markupCode), WithUsings(expected));
    }
}
