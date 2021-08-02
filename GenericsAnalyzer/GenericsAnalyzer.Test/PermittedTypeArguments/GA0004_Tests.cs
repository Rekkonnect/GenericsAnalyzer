using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0004_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void InvalidTypeArguments()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int*), ↓typeof(void))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
