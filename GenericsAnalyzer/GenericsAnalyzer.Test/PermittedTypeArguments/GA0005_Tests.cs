using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0005_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0005_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void InvalidTypeArguments()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;
using System.Collections.Generic;

class C
<
    [PermittedTypes(↓typeof(string))]
    [PermittedBaseTypes(↓typeof(IEnumerable<>))]
    [PermittedTypes(typeof(IList<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            AssertDiagnostics(testCode);
        }
    }
}
