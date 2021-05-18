using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0016_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0016_Rule;

        [TestMethod]
        public void RedundantUsageInClass()
        {
            var testCode =
@"
class C<T>
{
}
class D
<
    T,
    [↓InheritBaseTypeUsageConstraints]
    U
> : C<T>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
