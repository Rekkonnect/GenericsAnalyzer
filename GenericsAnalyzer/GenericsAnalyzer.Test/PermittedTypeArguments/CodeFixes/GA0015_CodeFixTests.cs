using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0015_CodeFixTests : RedundantAttributeRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
class C
<
    [{|*:InheritBaseTypeUsageConstraints|}]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
