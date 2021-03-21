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
        public void DuplicateConstraints()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int), typeof(long))]
    [PermittedTypes(↓typeof(int))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void MultiplicateConstraints()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int), typeof(long))]
    [PermittedTypes(↓typeof(int), ↓typeof(int), ↓typeof(int))]
    [PermittedTypes(↓typeof(int), ↓typeof(int))]
    [PermittedTypes(↓typeof(int), ↓typeof(int), ↓typeof(int),  ↓typeof(int), ↓typeof(int))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void MultipleDuplicateConstraints()
        {
            var testCode =
@"
class D
<
    [PermittedTypes(↓typeof(int), ↓typeof(long))]
    [PermittedTypes(↓typeof(int), typeof(short), ↓typeof(long))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
