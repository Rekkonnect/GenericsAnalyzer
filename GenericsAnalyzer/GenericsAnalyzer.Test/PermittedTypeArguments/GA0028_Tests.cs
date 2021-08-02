using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0028_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void MultipleDistinctGroupUsages()
        {
            var testCode =
$@"
[TypeConstraintProfileGroup]
interface IGroup0 {{ }}
[TypeConstraintProfileGroup]
interface IGroup1 {{ }}
[TypeConstraintProfileGroup]
interface IGroup2 {{ }}

[↓TypeConstraintProfile(typeof(IGroup0), typeof(IGroup1), typeof(IGroup2))]
interface IB {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
