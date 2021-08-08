using GenericsAnalyzer.Core;
using GenericsAnalyzer.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
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
    }
}
