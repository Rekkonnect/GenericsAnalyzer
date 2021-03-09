using Microsoft.CodeAnalysis;
using RoslynTestKit;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseCodeFixDiagnosticTests : CodeFixTestFixture, IAnalyzerTestFixture
    {
        public abstract DiagnosticDescriptor TestedDiagnosticRule { get; }

        public void TestCodeFix(string markupCode, string expected, int codeFixIndex = 0)
        {
            TestCodeFix(markupCode, expected, TestedDiagnosticRule.Id, codeFixIndex);
        }
    }
}
