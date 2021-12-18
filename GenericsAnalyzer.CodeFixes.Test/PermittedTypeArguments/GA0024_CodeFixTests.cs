using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0024_CodeFixTests : InstanceTypeMemberRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
[TypeConstraintProfileGroup]
interface {|*:IGroup0|}
{
    int Property { get; set; }
    void Function();
    interface INested { }
}
";

            var fixedCode =
@"
[TypeConstraintProfileGroup]
interface IGroup0
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
