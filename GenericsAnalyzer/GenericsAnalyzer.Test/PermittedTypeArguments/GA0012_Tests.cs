using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0012_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0012_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void MultipleConstraints()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;
using System.Collections.Generic;

class C
<
    [ProhibitedTypes(typeof(int), typeof(long))]
    [ProhibitedBaseTypes(typeof(IEnumerable<int>), typeof(ICollection<string>))]
    [↓OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnostics(testCode);
        }
    }
}
