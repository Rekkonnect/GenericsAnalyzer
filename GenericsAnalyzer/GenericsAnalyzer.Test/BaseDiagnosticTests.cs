using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GenericsAnalyzer.Test.Helpers.UsingsHelpers;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseDiagnosticTests<TAnalyzer> : BaseDiagnosticTests
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        protected sealed override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new TAnalyzer();
    }

    public abstract class BaseDiagnosticTests : BaseAnalyzerTestFixture
    {
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

        protected void AssertOrValidate(string testCode, bool assert)
        {
            if (assert)
                AssertDiagnostics(testCode);
            else
                ValidateCode(testCode.Replace("↓", ""));
        }
        protected void AssertOrValidateWithUsings(string testCode, bool assert)
        {
            AssertOrValidate(WithUsings(testCode), assert);
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
