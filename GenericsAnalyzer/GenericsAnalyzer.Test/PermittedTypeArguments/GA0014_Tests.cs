using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0014_Tests : BaseAnalyzerTests
    {
        protected override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0014_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        // Redundant usage of the attribute
        [TestMethod]
        public void RedundantUsageInFunctionAndDelegate()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

class C
<
    [InheritBaseTypeUsageConstraints]
    T0
>
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
