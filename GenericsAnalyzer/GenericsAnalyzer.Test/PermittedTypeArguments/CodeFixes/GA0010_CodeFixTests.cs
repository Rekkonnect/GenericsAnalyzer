using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0010_CodeFixTests : RedundantAttributeArgumentRemoverCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0010_Rule;

        [TestMethod]
        public void RedundantlyProhibitedTypeWithCodeFix()
        {
            var testCode =
@"
class C
<
    [ProhibitedTypes({|GA0010:typeof(long)|})]
    [ProhibitedBaseTypes(typeof(IComparable<>))]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    [ProhibitedBaseTypes(typeof(IComparable<>))]
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
    [Example, ProhibitedTypes({|GA0010:typeof(long)|}), Example]
    [ProhibitedBaseTypes(typeof(IComparable<>))]
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
    [ProhibitedBaseTypes(typeof(IComparable<>))]
    T
>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
