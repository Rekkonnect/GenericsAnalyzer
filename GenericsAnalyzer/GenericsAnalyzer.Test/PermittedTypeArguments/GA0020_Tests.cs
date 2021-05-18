using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0020_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0020_Rule;

        [TestMethod]
        public void RecursiveInheritance()
        {
            var testCode =
@"
class C
<
    // diagnostic is only emitted on the first parameter that causes the recursion
    [InheritTypeConstraints(↓nameof(U))]
    T,
    [InheritTypeConstraints(nameof(T))]
    U
>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IndirectRecursiveInheritance()
        {
            var testCode =
@"
class C
<
    [InheritTypeConstraints(nameof(U))]
    T,
    [InheritTypeConstraints(↓nameof(V))]
    U,
    [InheritTypeConstraints(nameof(U))]
    V
>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
