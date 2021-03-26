using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0022_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0022_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void ConflictingBaseTypeParameterConstraintRules()
        {
            var testCode =
@"
class A
<
    [PermittedBaseTypes(typeof(IEnumerable<string>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{ }

class B
<
    [ProhibitedBaseTypes(typeof(IEnumerable<string>))]
    [OnlyPermitSpecifiedTypes]
    T,
    [InheritTypeConstraints(↓nameof(T))]
    [↓InheritBaseTypeUsageConstraints]
    U
>
    : A<U>
{ }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
