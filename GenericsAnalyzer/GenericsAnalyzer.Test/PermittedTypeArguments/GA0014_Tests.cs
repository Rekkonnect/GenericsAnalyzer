using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0014_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0014_Rule;

        [TestMethod]
        public void RedundantUsageInFunctionAndDelegate()
        {
            var testCode =
@"
class A<T> { }
class C
<
    [InheritBaseTypeUsageConstraints]
    T0
> : A<T0>
{
    void Function
    <
        [↓InheritBaseTypeUsageConstraints]
        T1
    >()
    {
    }
}

delegate void Function
<
    [↓InheritBaseTypeUsageConstraints]
    T
>();
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
