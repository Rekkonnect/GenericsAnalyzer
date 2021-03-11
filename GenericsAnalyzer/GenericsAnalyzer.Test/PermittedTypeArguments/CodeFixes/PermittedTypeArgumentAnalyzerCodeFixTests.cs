using Microsoft.CodeAnalysis.CodeFixes;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    public abstract class PermittedTypeArgumentAnalyzerCodeFixTests<TCodeFix> : BaseCodeFixDiagnosticTests<PermittedTypeArgumentAnalyzer, TCodeFix>
        where TCodeFix : CodeFixProvider, new()
    {
    }
}
