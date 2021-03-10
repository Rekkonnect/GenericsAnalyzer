using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Threading.Tasks;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseCodeFixDiagnosticTests<TAnalyzer, TCodeFix> : IAnalyzerTestFixture
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public abstract DiagnosticDescriptor TestedDiagnosticRule { get; }

        public void TestCodeFix(string markupCode, string expected) => Task.WaitAll(TestCodeFixAsync(markupCode, expected));
        public async Task TestCodeFixAsync(string markupCode, string expected)
        {
            await CSharpCodeFixVerifier<TAnalyzer, TCodeFix>.VerifyCodeFixAsync(markupCode, expected);
        }
    }
}
