using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0017_Tests : BaseAnalyzerTests
    {
        protected override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0017_Rule;

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

            ValidateCode(testCode);
        }

        [TestMethod]
        public void IntermediateSubsetGenericTypeUsage()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void IntermediateExplicitlyPermittedGenericType()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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
";

            ValidateCode(testCode);
        }

        [TestMethod]
        public void IntermediateProhibitedGenericType()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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

            ValidateCode(testCode);
        }
    }
}
