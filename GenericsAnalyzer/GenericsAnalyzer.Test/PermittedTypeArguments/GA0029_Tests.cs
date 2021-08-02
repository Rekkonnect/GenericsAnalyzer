using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0029_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void InvalidProfileInterfaces()
        {
            var testCode =
$@"
[TypeConstraintProfileGroup(false)]
interface IGroup0 {{ }}
[TypeConstraintProfileGroup(false)]
interface IGroup1 {{ }}
[TypeConstraintProfileGroup(false)]
interface IGroup2 {{ }}

[TypeConstraintProfile]
[TypeConstraintProfileGroup]
interface ↓IA {{ }}

[TypeConstraintProfile(typeof(IGroup0), typeof(IGroup1), typeof(IGroup2))]
[TypeConstraintProfileGroup(false)]
interface ↓IB {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
