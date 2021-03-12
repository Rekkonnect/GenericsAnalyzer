using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test
{
    public abstract class BaseDiagnosticTests : IAnalyzerTestFixture
    {
        private const string usings =
@"
using GenericsAnalyzer.Core;
using System;
using System.Collections.Generic;
";

        public abstract DiagnosticDescriptor TestedDiagnosticRule { get; }

        protected ExpectedDiagnostic ExpectedDiagnostic => ExpectedDiagnostic.Create(TestedDiagnosticRule);

        protected abstract DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance();

        protected void ValidateCode(string testCode)
        {
            RoslynAssert.Valid(GetNewDiagnosticAnalyzerInstance(), testCode);
        }
        protected void ValidateCodeWithUsings(string testCode)
        {
            ValidateCode($"{usings}\n{testCode}");
        }
        protected void AssertDiagnostics(string testCode)
        {
            RoslynAssert.Diagnostics(GetNewDiagnosticAnalyzerInstance(), ExpectedDiagnostic, testCode);
        }
        protected void AssertDiagnosticsWithUsings(string testCode)
        {
            AssertDiagnostics($"{usings}\n{testCode}");
        }

        [TestMethod]
        public void EmptyCode()
        {
            ValidateCode(@"");
        }
        [TestMethod]
        public void EmptyCodeWithUsings()
        {
            ValidateCode(usings);
        }
    }
}
