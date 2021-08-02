using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0015_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void RedundantUsageInClass()
        {
            var testCode =
@"
class Base { }
class A
<
    [↓InheritBaseTypeUsageConstraints]
    T
> : Base
{
}
class C
<
    [↓InheritBaseTypeUsageConstraints]
    T
>
{
}
class D
<
    [InheritBaseTypeUsageConstraints]
    T,
    U
> : C<T>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
