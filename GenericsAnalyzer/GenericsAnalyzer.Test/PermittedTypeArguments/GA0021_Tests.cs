using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0021_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0021_Rule;

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
