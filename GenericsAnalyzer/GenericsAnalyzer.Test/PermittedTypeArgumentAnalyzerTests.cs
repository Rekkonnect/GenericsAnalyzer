using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test
{
    [TestClass]
    public class PermittedTypeArgumentAnalyzerTests : BaseAnalyzerTests
    {
        protected override string TestedDiagnosticID => PermittedTypeArgumentAnalyzer.DiagnosticID;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        // No diagnostics expected to show up
        [TestMethod]
        public void EmptyCode()
        {
            ValidateCode(@"");
        }

        // Usage of type arguments in inheritance
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

        // Usage of prohibited type arguments for class
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

        // Usage of prohibited type arguments for function
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

        // Usage of prohibited type arguments for class with OnlyPermitSpecifiedTypesAttribute
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
