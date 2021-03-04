using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0001_Tests : BaseAnalyzerTests
    {
        protected override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0001_Rule;

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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void IntermediateExplicitlyPermittedGenericTypeUsage()
        {
            var testCode =
@"
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void IntermediateProhibitedGenericTypeUsage()
        {
            var testCode =
@"
using System;
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void IntermediateTwoClassProhibitedGenericTypeUsage()
        {
            var testCode =
@"
using System;
using GenericsAnalyzer.Core;

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
    T
> : A<T>
{
}
class C
<
    [InheritBaseTypeUsageConstraints]
    T
> : B<T>
{
}
";

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void GenericClassTestCode()
        {
            var testCode =
@"
using System;
using GenericsAnalyzer.Core;

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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void GenericFunctionTestCode()
        {
            var testCode =
@"
using System;
using GenericsAnalyzer.Core;

class Program
{
    static void Main()
    {
        Function<int, int>();
        Function<↓string, int>();
        Function<int, ↓ulong>();
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

            AssertDiagnostics(testCode);
        }

        [TestMethod]
        public void OnlyPermitSpecifiedTypesTestCode()
        {
            var testCode =
@"
using System;
using System.Collections.Generic;
using GenericsAnalyzer.Core;

class Program
{
    static void Main()
    {
        new Generic<int>();
        new Generic<long>();
        new Generic<string>();
        new Generic<↓ulong>();
        new Generic<↓byte>();
    }
}

class Generic
<
    [PermittedTypes(typeof(int), typeof(long))]
    [PermittedBaseTypes(typeof(IEnumerable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnostics(testCode);
        }
    }
}
