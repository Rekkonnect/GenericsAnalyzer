using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0014_CodeFixTests : RedundantAttributeRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
delegate void Delegate
<
    [{|GA0014:InheritBaseTypeUsageConstraints|}]
    T
>(T something);
";

            var fixedCode =
@"
delegate void Delegate
<
    T
>(T something);
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        // This is only tested in GA0014 since it's the same code fix
        [TestMethod]
        public void AttributeListRedundantUsageWithCodeFix()
        {
            var testCode =
@"
delegate void Delegate
<
    [Example, {|GA0014:InheritBaseTypeUsageConstraints|}, Example]
    T
>(T something);
";

            var fixedCode =
@"
delegate void Delegate
<
    [Example, Example]
    T
>(T something);
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
