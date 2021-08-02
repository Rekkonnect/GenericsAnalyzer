using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public class PermittedTypeArgumentAnalyzerDiagnosticUnboundTests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        // This class should only contain tests that pass and do not report any diagnostics we care about
        // Hence, the potentially breaking behavior of using null as the rule should never be experienced
        public sealed override DiagnosticDescriptor TestedDiagnosticRule => null;

        [TestMethod]
        public void ArrayTypeUsage()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int[]>();
    }
}

class A<T>
{
}
";

            ValidateCodeWithUsings(testCode);
        }

        [TestMethod]
        public void EmptyPermissionAttributeArgumentListTest()
        {
            // This code should normally throw some diagnostic, though it is intended
            // to be a feature included in another analyzer regading method usage
            // In this case, the analyzer would be used to prohibit usage of empty params 

            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int[]>();
    }
}

class A
<
    [PermittedTypes]
    [PermittedBaseTypes]
    [ProhibitedTypes]
    [ProhibitedBaseTypes]
    T
>
{
}
";

            ValidateCodeWithUsings(testCode);
        }

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

        #region Reducibility to constraint clause
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
        #endregion
    }
}
