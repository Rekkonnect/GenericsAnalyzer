using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0016_CodeFixTests : PermittedTypeArgumentAnalyzerCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0016_Rule;

        protected override string LanguageName => LanguageNames.CSharp;

        protected override CodeFixProvider CreateProvider() => new InheritBaseTypeUsageConstraintsAttributeRemover();

        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

class A<T> { }
class C
<
    [[|InheritBaseTypeUsageConstraints|]]
    T
>
    : A<int>
{
}
";

            var fixedCode =
@"
using GenericsAnalyzer.Core;

class A<T> { }
class C
<
    T
>
    : A<int>
{
}
";

            TestCodeFix(testCode, fixedCode);
        }
    }
}
