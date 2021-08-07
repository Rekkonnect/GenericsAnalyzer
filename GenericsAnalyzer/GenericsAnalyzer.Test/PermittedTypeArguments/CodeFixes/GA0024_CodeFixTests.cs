using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
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
interface {|GA0024:IGroup0|}
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
