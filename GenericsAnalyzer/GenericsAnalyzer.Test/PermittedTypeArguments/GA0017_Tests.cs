using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0017_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0017_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void IntermediateGenericTypeUsage()
        {
            var testCode =
@"
class A<T>
{
}
class B<T> : A<T>
{
}
";

            ValidateCodeWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateSubsetGenericTypeUsage()
        {
            var testCode =
@"
class A
<
    [ProhibitedBaseTypes(typeof(IA))]
    T
>
{
}
class B
<
    [ProhibitedBaseTypes(typeof(IB))]
    T
> : A<T>
{
}
class C
<
    [ProhibitedBaseTypes(typeof(IC))]
    T
> : A<↓T>
{
}

interface IA : IB { }
interface IB { }
interface IC : IA { }

class A : IA { }
class B : IB { }
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateExplicitlyPermittedGenericType()
        {
            var testCode =
@"
class A
<
    [PermittedTypes(typeof(int), typeof(uint))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
class B
<
    [InheritBaseTypeUsageConstraints]
    T
> : A<T>
{
}
class C<T> : A<↓T>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateProhibitedGenericType()
        {
            var testCode =
@"
class A
<
    [ProhibitedTypes(typeof(string))]
    T
>
{
}
class B
<
    [InheritBaseTypeUsageConstraints]
    T
> : A<T>
{
}
";

            ValidateCodeWithUsings(testCode);
        }

        [TestMethod]
        public void SubsetVarianceTest()
        {
            var testCode =
@"
class A
<
    [PermittedBaseTypes(typeof(ID))]
    [ProhibitedBaseTypes(typeof(IA), typeof(IC))]
    T
>
{
}
class B
<
    [ProhibitedBaseTypes(typeof(IBase))]
    T
> : A<T>
{
}

interface IBase { }
interface IA : IBase { }
interface IB : IBase { }
interface IC : IBase { }
interface ID : IC { }
";

            ValidateCodeWithUsings(testCode);
        }
    }
}
