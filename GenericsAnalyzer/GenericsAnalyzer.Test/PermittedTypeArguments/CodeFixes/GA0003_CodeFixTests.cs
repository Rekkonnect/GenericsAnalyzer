using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0003_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0003_Rule;

        [TestMethod]
        public void RedundantBoundUnboundCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|GA0003:typeof(IComparable<int>)|})]
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
    [PermittedTypes({|GA0003:typeof(IComparable<int>)|}, typeof(IComparable<>))]
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
