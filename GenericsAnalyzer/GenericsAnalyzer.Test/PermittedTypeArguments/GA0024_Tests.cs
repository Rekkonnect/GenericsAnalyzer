using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0024_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void ProfileGroupInterfaceWithInstanceMembers()
        {
            var testCode =
$@"
[TypeConstraintProfileGroup]
interface ↓IGroup0
{{
    int Property {{ get; set; }}
}}

[TypeConstraintProfileGroup]
interface ↓IGroup1
{{
    void Function();
}}

[TypeConstraintProfileGroup]
interface ↓IGroup2
{{
    interface INested {{ }}
}}

[TypeConstraintProfile(typeof(IGroup0), typeof(IGroup1))]
interface IB
{{
}}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
