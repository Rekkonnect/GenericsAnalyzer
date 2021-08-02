using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0019_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void InvalidTypeParameterNameClass()
        {
            var testCode =
@"
class C
<
    [InheritTypeConstraints(↓""T1"", ""U"", nameof(U))]
    T,
    U
>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void InvalidTypeParameterNameFunction()
        {
            var testCode =
@"
class C
{
    void Function
    <
        [InheritTypeConstraints(↓""T1"", ""U"")] // nameof is not yet legal here
        T,
        U
    >()
    { }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
