using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0002_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0002_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void ConflictingConstraints()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int), typeof(long))]
    [ProhibitedTypes(↓typeof(int))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void MultipleConflictingConstraints()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(List<int>), typeof(long))]
    [ProhibitedTypes(↓typeof(List<int>))]
    [ProhibitedBaseTypes(↓typeof(List<int>))]
    [PermittedBaseTypes(↓typeof(List<int>))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void MultipleConflictingDuplicateConstraints()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(List<int>), typeof(long))]
    [ProhibitedTypes(↓typeof(List<int>), ↓typeof(List<int>))]
    [ProhibitedBaseTypes(↓typeof(List<int>))]
    [PermittedBaseTypes(↓typeof(List<int>), ↓typeof(List<int>))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void MultipleDifferentConflictingConstraints()
        {
            var testCode =
@"
class D
<
    [PermittedTypes(↓typeof(int), ↓typeof(long[]))]
    [ProhibitedTypes(↓typeof(int), typeof(short), ↓typeof(long[]))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
