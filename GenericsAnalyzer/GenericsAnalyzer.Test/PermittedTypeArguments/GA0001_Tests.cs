using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0001_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void ImplicitFunctionTypeArgumentUsage()
        {
            var testCode =
@"
class Program
{
    void Run()
    {
        ↓Function(5); // implicit usage of 'int'
        Function<↓int>(5); // explicit usage of 'int'
    }

    void Function
    <
        [ProhibitedTypes(typeof(int))]
        T
    >
    (T v)
    {
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateSubsetGenericTypeUsage()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int>();
        new B<int>();
        new A<↓A>();
        new B<↓A>();
        new A<B>();
        new B<↓B>();
    }
}

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

interface IA : IB { }
interface IB { }

class A : IA { }
class B : IB { }
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateExplicitlyPermittedGenericTypeUsage()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int>();
        new B<int>();
        new A<↓string>();
        new B<↓string>();
    }
}

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

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateProhibitedGenericTypeUsage()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int>();
        new B<int>();
        new A<↓string>();
        new B<↓string>();
    }
}

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

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void IntermediateTwoClassProhibitedGenericTypeUsage()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new A<int>();
        new C<int>();
        new A<↓string>();
        new C<↓string>();
    }
}

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
    U
> : A<U>
{
}
class C
<
    [InheritBaseTypeUsageConstraints]
    V
> : B<V>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void GenericClassTestCode()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new Generic<int, int>();
        new Generic<↓string, int>();
        new Generic<int, ↓ulong>();
    }
}

class Generic
<
    [ProhibitedTypes(typeof(string))]
    TNotString,
    [PermittedTypes(typeof(int))]
    [ProhibitedBaseTypes(typeof(IComparable<>))]
    TNotComparableButInt
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void GenericFunctionTestCode()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        Function<int, int>();
        Function<↓string, int>();
        Function<int, ↓ulong>();
        Function<int, ↓IComparable<ulong>>();
    }

    static void Function
    <
        [ProhibitedTypes(typeof(string))]
        TNotString,
        [PermittedTypes(typeof(int))]
        [ProhibitedBaseTypes(typeof(IComparable<>))]
        TNotComparableButInt
    >
    ()
    {   
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void OnlyPermitSpecifiedTypesTestCode()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new Generic<int>();
        new Generic<long>();
        new Generic<string>();
        new Generic<IEnumerable<string>>();
        new Generic<↓ulong>();
        new Generic<↓byte>();
        new Generic<A<int>>();
        new Generic<B<int>>();
    }
}

class Generic
<
    [PermittedTypes(typeof(int), typeof(long))]
    [PermittedBaseTypes(typeof(IEnumerable<>))]
    [PermittedBaseTypes(typeof(A<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}

class A<T> { }
class B<T> : A<T> { }
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void PartialClassTestCode()
        {
            var testCode =
@"
class Program
{
    static void Main()
    {
        new Generic<int>();
        new Generic<long>();
        new Generic<string>();
        new Generic<IEnumerable<string>>();
        new Generic<↓ulong>();
        new Generic<↓byte>();
    }
}

partial class Generic
<
    [PermittedBaseTypes(typeof(IEnumerable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}

partial class Generic
<
    [PermittedTypes(typeof(int), typeof(long))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
