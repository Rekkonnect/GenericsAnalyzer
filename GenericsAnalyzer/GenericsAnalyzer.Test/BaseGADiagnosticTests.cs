using GenericsAnalyzer.AnalysisTestsBase.Helpers;
using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoseLynn.Analyzers;
using RoseLynn.Testing;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseGADiagnosticTests<TAnalyzer> : BaseGADiagnosticTests
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        protected sealed override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new TAnalyzer();
    }

    public abstract class BaseGADiagnosticTests : BaseDiagnosticTests
    {
        protected ExpectedDiagnostic ExpectedDiagnostic => ExpectedDiagnostic.Create(TestedDiagnosticRule);
        protected sealed override DiagnosticDescriptorStorageBase DiagnosticDescriptorStorage => GADiagnosticDescriptorStorage.Instance;

        protected override UsingsProviderBase GetNewUsingsProviderInstance()
        {
            return GAUsingsProvider.Instance;
        }

        protected override void ValidateCode(string testCode)
        {
            RoslynAssert.Valid(GetNewDiagnosticAnalyzerInstance(), testCode);
        }
        protected override void AssertDiagnostics(string testCode)
        {
            RoslynAssert.Diagnostics(GetNewDiagnosticAnalyzerInstance(), ExpectedDiagnostic, testCode);
        }

        [TestMethod]
        public void EmptyCode()
        {
            ValidateCode(@"");
        }
        [TestMethod]
        public void EmptyCodeWithUsings()
        {
            ValidateCode(GAUsingsProvider.DefaultUsings);
        }
    }
}
