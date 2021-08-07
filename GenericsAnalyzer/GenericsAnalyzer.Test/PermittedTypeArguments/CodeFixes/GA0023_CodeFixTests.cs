using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0023_CodeFixTests : UngenericizerCodeFixTests
    {
        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
[TypeConstraintProfileGroup]
interface {|GA0023:IProfile|}<T1, T2, T3> { }
";

            var fixedCode =
@"
[TypeConstraintProfileGroup]
interface IProfile { }
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
