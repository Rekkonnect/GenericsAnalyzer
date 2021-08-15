using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0025_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void UnrelatedProfileInterfaces()
        {
            var testCode =
$@"
[TypeConstraintProfile]
interface IA {{ }}

interface IB : ↓IA {{ }}

[TypeConstraintProfile]
interface IC : ↓IB {{ }}

[TypeConstraintProfile]
interface ID : IC {{ }}

interface IE {{ }}

[TypeConstraintProfile]
interface IF : IA, ↓IB, ↓IE {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void ProfileInterfacesAndProfileGroupInterfaces()
        {
            var testCode =
$@"
[TypeConstraintProfile]
interface IA {{ }}

[TypeConstraintProfileGroup]
interface IB : ↓IA {{ }}

[TypeConstraintProfile]
interface IC : ↓IB {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void PartialInterfaceInheritingMixed()
        {
            var testCode =
$@"
[TypeConstraintProfile]
interface IA {{ }}

interface IB {{ }}

[TypeConstraintProfile]
interface IC {{ }}

[TypeConstraintProfile]
interface ID {{ }}

[TypeConstraintProfile]
partial interface IE : IA, IC, ↓IB {{ }}
partial interface IE : ↓IB, ID, IA {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
