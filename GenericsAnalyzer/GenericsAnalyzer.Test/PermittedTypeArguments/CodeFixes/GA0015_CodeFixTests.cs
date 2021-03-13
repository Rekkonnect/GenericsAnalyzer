using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0015_CodeFixTests : RedundantAttributeRemoverCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0015_Rule;

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
