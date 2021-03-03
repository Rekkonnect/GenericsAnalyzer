using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseAnalyzerTests
    {
        protected abstract DiagnosticDescriptor TestedDiagnosticRule { get; }

        protected ExpectedDiagnostic ExpectedDiagnostic => ExpectedDiagnostic.Create(TestedDiagnosticRule);

        protected abstract DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance();

        protected void ValidateCode(string testCode)
        {
            RoslynAssert.Valid(GetNewDiagnosticAnalyzerInstance(), testCode);
        }
        protected void AssertDiagnostics(string testCode)
        {
            RoslynAssert.Diagnostics(GetNewDiagnosticAnalyzerInstance(), ExpectedDiagnostic, testCode);
        }
    }
}
