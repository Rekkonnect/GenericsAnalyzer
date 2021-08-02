using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0021_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void SameTypeParameterInheritance()
        {
            var testCode =
@"
class C
<
    [InheritTypeConstraints(↓nameof(T), ↓""T"")]
    T
>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
