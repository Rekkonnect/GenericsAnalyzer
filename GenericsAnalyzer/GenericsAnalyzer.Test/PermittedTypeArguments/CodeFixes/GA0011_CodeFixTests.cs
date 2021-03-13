using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0011_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0011_Rule;

        [TestMethod]
        public void RedundantlyProhibitedTypeWithCodeFix()
        {
            var testCode =
@"
class C
<
    [PermittedTypes({|GA0011:typeof(long)|})]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
    [PermittedBaseTypes(typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void RedundantlyProhibitedTypeInAttributeListWithCodeFix()
        {
            var testCode =
@"
class C
<
    [Example, PermittedTypes({|GA0011:typeof(long)|}), Example]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
    [Example, Example]
    [PermittedBaseTypes(typeof(IComparable<>))]
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
