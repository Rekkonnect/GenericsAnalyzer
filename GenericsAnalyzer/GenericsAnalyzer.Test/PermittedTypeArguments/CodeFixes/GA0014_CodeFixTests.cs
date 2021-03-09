using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0014_CodeFixTests : PermittedTypeArgumentAnalyzerCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0014_Rule;

        protected override string LanguageName => LanguageNames.CSharp;

        protected override CodeFixProvider CreateProvider() => new InheritBaseTypeUsageConstraintsAttributeRemover();

        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

delegate void Delegate
<
    [[|InheritBaseTypeUsageConstraints|]]
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
    }
}
