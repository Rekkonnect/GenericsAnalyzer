using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0016_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0016_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

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
