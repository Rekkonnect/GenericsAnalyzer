using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0005_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        [TestMethod]
        public void ConstrainedTypeArgumentCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(string)|})]
    [PermittedTypes(typeof(IList<int>), typeof(ISet<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            var fixedCode =
@"
class C
<
    [PermittedTypes(typeof(IList<int>), typeof(ISet<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ConstrainedTypeArgumentMultipleTypeRulesCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|*:typeof(string)|}, typeof(IList<int>), typeof(ISet<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            var fixedCode =
@"
class C
<
    [PermittedTypes(typeof(IList<int>), typeof(ISet<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
