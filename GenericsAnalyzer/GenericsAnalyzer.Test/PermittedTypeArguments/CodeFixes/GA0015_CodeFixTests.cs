using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0015_CodeFixTests : PermittedTypeArgumentAnalyzerCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0015_Rule;

        protected override string LanguageName => LanguageNames.CSharp;

        protected override CodeFixProvider CreateProvider() => new InheritBaseTypeUsageConstraintsAttributeRemover();

        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

class C
<
    [[|InheritBaseTypeUsageConstraints|]]
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
