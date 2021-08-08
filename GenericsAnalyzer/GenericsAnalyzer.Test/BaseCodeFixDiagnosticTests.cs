using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseCodeFixDiagnosticTests<TAnalyzer, TCodeFix> : BaseAnalyzerTestFixture
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : MultipleDiagnosticCodeFixProvider, new()
    {
        public void TestCodeFix(string markupCode, string expected, int codeActionIndex = 0) => Task.WaitAll(TestCodeFixAsync(markupCode, expected, codeActionIndex));
        public async Task TestCodeFixAsync(string markupCode, string expected, int codeActionIndex = 0)
        {
            markupCode = markupCode.Replace("{|*", $"{{|{TestedDiagnosticRule.Id}");
            await CSharpCodeFixVerifier<TAnalyzer, TCodeFix>.VerifyCodeFixAsync(markupCode, expected, codeActionIndex);
        }

        [TestMethod]
        public void TestExistingCodeFixName()
        {
            Assert.IsNotNull(new TCodeFix().CodeFixTitle);
        }
    }
}
