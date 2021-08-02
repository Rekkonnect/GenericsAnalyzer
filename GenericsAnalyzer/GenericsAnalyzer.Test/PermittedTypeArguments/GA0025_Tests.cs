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
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
