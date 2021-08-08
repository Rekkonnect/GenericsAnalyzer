using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0011_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantlyProhibitedTypeWithCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(long)|})]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
    [PermittedBaseTypes(typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void RedundantlyProhibitedTypeInAttributeListWithCodeFix()
        {
            var testCode =
@"
class C
<
    [Example, PermittedTypes({|*:typeof(long)|}), Example]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
    [Example, Example]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
