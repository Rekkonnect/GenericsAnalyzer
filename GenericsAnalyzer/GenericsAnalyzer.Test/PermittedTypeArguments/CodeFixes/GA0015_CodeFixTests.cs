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
    [{|GA0015:InheritBaseTypeUsageConstraints|}]
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
