using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GenericsAnalyzer.Test.Helpers.UsingsHelpers;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseDiagnosticTests : IAnalyzerTestFixture
    {
        public abstract DiagnosticDescriptor TestedDiagnosticRule { get; }

        protected ExpectedDiagnostic ExpectedDiagnostic => ExpectedDiagnostic.Create(TestedDiagnosticRule);

        protected abstract DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance();

        protected void ValidateCode(string testCode)
        {
            RoslynAssert.Valid(GetNewDiagnosticAnalyzerInstance(), testCode);
        }
        protected void ValidateCodeWithUsings(string testCode)
        {
            ValidateCode(WithUsings(testCode));
        }
        protected void AssertDiagnostics(string testCode)
        {
            RoslynAssert.Diagnostics(GetNewDiagnosticAnalyzerInstance(), ExpectedDiagnostic, testCode);
        }
        protected void AssertDiagnosticsWithUsings(string testCode)
        {
            AssertDiagnostics(WithUsings(testCode));
        }

        [TestMethod]
        public void EmptyCode()
        {
            ValidateCode(@"");
        }
        [TestMethod]
        public void EmptyCodeWithUsings()
        {
            ValidateCode(DefaultNecessaryUsings);
        }
    }
}
