using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0023_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0023_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void ConflictingConstraintRules()
        {
            var testCode =
@"
class A
<
    [PermittedBaseTypes(typeof(IEnumerable<string>))]
    [OnlyPermitSpecifiedTypes]
    T,
    [ProhibitedBaseTypes(typeof(IEnumerable<string>))]
    [InheritTypeConstraints(↓nameof(T), ↓""T"")]
    U
>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
