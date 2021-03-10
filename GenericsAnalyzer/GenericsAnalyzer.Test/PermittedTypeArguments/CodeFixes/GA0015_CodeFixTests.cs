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
using GenericsAnalyzer.Core;

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
using GenericsAnalyzer.Core;

class C
<
    T
>
{
}
";

            TestCodeFix(testCode, fixedCode);
        }
    }
}
