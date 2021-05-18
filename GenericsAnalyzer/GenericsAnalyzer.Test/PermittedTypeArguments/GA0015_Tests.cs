using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0015_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0015_Rule;

        [TestMethod]
        public void RedundantUsageInClass()
        {
            var testCode =
@"
class Base { }
class A
<
    [↓InheritBaseTypeUsageConstraints]
    T
> : Base
{
}
class C
<
    [↓InheritBaseTypeUsageConstraints]
    T
>
{
}
class D
<
    [InheritBaseTypeUsageConstraints]
    T,
    U
> : C<T>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
