using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0030_CodeFixTests : ProfileInterfaceDeclarerCodeFixTests
    {
        [TestMethod]
        public void ConstraintAttributeProfileGroupWithCodeFix()
        {
            var testCode =
@"
[TypeConstraintProfileGroup]
[{|*:PermittedTypes(typeof(int))|}]
interface I
{
}
";

            var fixedCode =
@"
[TypeConstraintProfile]
[PermittedTypes(typeof(int))]
interface I
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void SurroundedConstraintAttributeProfileGroupWithCodeFix()
        {
            var testCode =
@"
[Example, TypeConstraintProfileGroup, Example]
[{|*:PermittedTypes(typeof(int))|}]
interface I
{
}
";

            var fixedCode =
@"
[Example, TypeConstraintProfile, Example]
[PermittedTypes(typeof(int))]
interface I
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ConstrainedProfileIrrelevantInterfaceWithCodeFix()
        {
            var testCode =
@"
[Example, Example]
[{|*:PermittedTypes(typeof(int))|}]
interface I
{
}
";

            var fixedCode =
@"
[Example, Example]
[PermittedTypes(typeof(int))]
[TypeConstraintProfile]
interface I
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
