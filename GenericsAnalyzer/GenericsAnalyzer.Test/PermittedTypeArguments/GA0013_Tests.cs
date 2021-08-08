using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0013_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void MultipleConstraints()
        {
            var testCode =
@"
class C
<
    [↓PermittedTypes(typeof(int))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
