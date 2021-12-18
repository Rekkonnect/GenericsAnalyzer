using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0003_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantBoundUnboundCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(IComparable<int>)|})]
    [PermittedTypes(typeof(IComparable<>))]
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
    [PermittedTypes(typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void RedundantBaseTypeRuleWithinMultipleTypeRulesWithCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(IComparable<int>)|}, typeof(IComparable<>))]
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
    [PermittedTypes(typeof(IComparable<>))]
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
