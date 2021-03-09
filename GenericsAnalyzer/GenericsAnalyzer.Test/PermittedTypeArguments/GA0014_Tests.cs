using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0014_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0014_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void RedundantUsageInFunctionAndDelegate()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }
    }
}
