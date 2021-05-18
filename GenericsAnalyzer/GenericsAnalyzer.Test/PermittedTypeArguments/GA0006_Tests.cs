using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0006_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0006_Rule;

        [TestMethod]
        public void Reducible()
        {
            var testCode =
@"
class C
<
    [PermittedBaseTypes(↓typeof(IComparable<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void IrreducibleUnboundGeneric()
        {
            var testCode =
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

            ValidateCodeWithUsings(testCode);
        }
        [TestMethod]
        public void DeceptivelyReducible()
        {
            var testCode =
@"
#pragma warning disable GA0011
class C
<
    [PermittedTypes(typeof(int), typeof(long))]
    [PermittedBaseTypes(typeof(IComparable<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            ValidateCodeWithUsings(testCode);
        }
        [TestMethod]
        public void GenerallyIrreducible()
        {
            var testCode =
@"
#pragma warning disable GA0011
class C
<
    [PermittedBaseTypes(typeof(IComparable<int>))]
    T
>
{
}
";

            ValidateCodeWithUsings(testCode);
        }
        [TestMethod]
        public void ReducibleClass()
        {
            var testCode =
@"
class Test
<
    [PermittedBaseTypes(↓typeof(C))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : A
{
}

class A { }
class B : A { }
class C : B { }
class D : C { }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
