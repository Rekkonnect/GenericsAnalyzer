using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0004_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        [TestMethod]
        public void InvalidTypeArgumentCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(int*)|})]
    [ProhibitedBaseTypes(typeof(object))]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    [ProhibitedBaseTypes(typeof(object))]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void InvalidTypeArgumentMultipleTypeRulesCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(int*)|}, typeof(List<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    [PermittedTypes(typeof(List<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
