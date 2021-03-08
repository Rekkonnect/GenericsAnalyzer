using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0016_Tests : BaseAnalyzerTests
    {
        protected override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0016_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void RedundantUsageInClass()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }
    }
}
