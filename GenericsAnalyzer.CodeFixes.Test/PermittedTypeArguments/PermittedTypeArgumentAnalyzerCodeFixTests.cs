using GenericsAnalyzer.AnalysisTestsBase.Helpers;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    public abstract class PermittedTypeArgumentAnalyzerCodeFixTests<TCodeFix> : BaseCodeFixTests<PermittedTypeArgumentAnalyzer, TCodeFix>
        where TCodeFix : GACodeFixProvider, new()
    {
        public void TestCodeFixWithUsings(string markupCode, string expected, int codeActionIndex = 0)
        {
            TestCodeFix(GAUsingsProvider.Instance.WithUsings(markupCode), GAUsingsProvider.Instance.WithUsings(expected), codeActionIndex);
        }
    }
}
