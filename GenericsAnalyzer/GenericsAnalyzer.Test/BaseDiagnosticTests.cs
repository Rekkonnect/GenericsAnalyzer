using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis;
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

    public abstract class BaseDiagnosticTests : IAnalyzerTestFixture
    {
        private DiagnosticDescriptor testedDiagnosticRule;

        public virtual DiagnosticDescriptor TestedDiagnosticRule
        {
            get
            {
                if (testedDiagnosticRule != null)
                    return testedDiagnosticRule;

                // TODO: This will need major refactoring if a new diagnostic group will be introduced
                var thisType = GetType();
                var ruleID = thisType.Name.Substring(0, "GA0000".Length);
                return testedDiagnosticRule = DiagnosticDescriptors.GetDiagnosticDescriptor(ruleID);
            }
        }

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
