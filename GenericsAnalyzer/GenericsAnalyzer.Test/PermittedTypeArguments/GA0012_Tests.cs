using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0012_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0012_Rule;

        [TestMethod]
        public void MultipleConstraints()
        {
            var testCode =
@"
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

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
