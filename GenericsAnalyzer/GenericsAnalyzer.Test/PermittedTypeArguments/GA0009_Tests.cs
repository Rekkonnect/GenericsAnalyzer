using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0009_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0009_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void MultipleConstraints()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

class C
<
    [PermittedTypes(↓typeof(int), typeof(long))]
    [PermittedTypes(↓typeof(int))]
    T
>
{
}

class D
<
    [PermittedTypes(↓typeof(int), ↓typeof(long))]
    [PermittedTypes(↓typeof(int), typeof(short), ↓typeof(long))]
    T
>
{
}
";

            AssertDiagnostics(testCode);
        }
    }
}
