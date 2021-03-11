using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0014_CodeFixTests : RedundantAttributeRemoverCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0014_Rule;

        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

delegate void Delegate
<
    [{|GA0014:InheritBaseTypeUsageConstraints|}]
    T
>(T something);
";

            var fixedCode =
@"
using GenericsAnalyzer.Core;

delegate void Delegate
<
    T
>(T something);
";

            TestCodeFix(testCode, fixedCode);
        }
        // This is only tested in GA0014 since it's the same code fix
        [TestMethod]
        public void AttributeListRedundantUsageWithCodeFix()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;
using GenericsAnalyzer.Test.Resources;

delegate void Delegate
<
    [Example, {|GA0014:InheritBaseTypeUsageConstraints|}, Example]
    T
>(T something);
";

            var fixedCode =
@"
using GenericsAnalyzer.Core;
using GenericsAnalyzer.Test.Resources;

delegate void Delegate
<
    [Example, Example]
    T
>(T something);
";

            TestCodeFix(testCode, fixedCode);
        }
    }
}
