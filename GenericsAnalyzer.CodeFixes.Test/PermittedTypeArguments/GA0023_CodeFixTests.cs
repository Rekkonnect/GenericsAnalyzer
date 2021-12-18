using GenericsAnalyzer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoseLynn.CSharp.Syntax;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0023_CodeFixTests : UngenericizerCodeFixTests
    {
        [TestMethod]
        public void GenericProfileWithCodeFix()
        {
            GenericProfileRelatedInterfaceWithCodeFix(nameof(TypeConstraintProfileAttribute));
        }
        [TestMethod]
        public void GenericProfileGroupWithCodeFix()
        {
            GenericProfileRelatedInterfaceWithCodeFix(nameof(TypeConstraintProfileGroupAttribute));
        }

        private void GenericProfileRelatedInterfaceWithCodeFix(string attributeName)
        {
            ExtendedSyntaxFactory.SimplifyAttributeNameUsage(ref attributeName);

            var testCode =
$@"
[{attributeName}]
interface {{|*:IProfile1|}}<T1> {{ }}
[{attributeName}]
interface {{|*:IProfile2|}}<T1, T2> {{ }}
[{attributeName}]
interface {{|*:IProfile3|}}<T1, T2, T3> {{ }}
";

            var fixedCode =
$@"
[{attributeName}]
interface IProfile1 {{ }}
[{attributeName}]
interface IProfile2 {{ }}
[{attributeName}]
interface IProfile3 {{ }}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }

        [TestMethod]
        public void PartialGenericProfileInterfaceWithCodeFix()
        {
            var testCode =
$@"
[TypeConstraintProfile]
partial interface {{|*:IProfile1|}}<T1> {{ }}

partial interface IProfile1<T1> {{ }}
";

            var fixedCode =
$@"
[TypeConstraintProfile]
partial interface IProfile1 {{ }}

partial interface IProfile1 {{ }}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
