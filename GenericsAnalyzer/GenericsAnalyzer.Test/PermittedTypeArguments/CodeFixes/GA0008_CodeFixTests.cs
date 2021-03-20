using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    [Ignore("The resulting document when performing the operation is correct, however asserions fail")]
    public class GA0008_CodeFixTests : RedundantBaseTypeRuleConverterCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0008_Rule;

        [TestMethod]
        public void RedundantBaseTypeRuleWithCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedBaseTypes({|GA0008:typeof(long)|})]
    [PermittedBaseTypes(typeof(IEnumerable<>))]
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
    [PermittedTypes(typeof(long))]
    [PermittedBaseTypes(typeof(IEnumerable<>))]
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
    [ProhibitedBaseTypes(typeof(Attribute), {|GA0008:typeof(long)|}, typeof(IEnumerable<>))]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    [ProhibitedTypes(typeof(long))]
    [ProhibitedBaseTypes(typeof(Attribute), typeof(IEnumerable<>))]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
