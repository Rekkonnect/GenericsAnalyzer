using Gu.Roslyn.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test
{
    [TestClass]
    public class PermittedTypeArgumentAnalyzerTests
    {
        // No diagnostics expected to show up
        [TestMethod]
        public void EmptyCode()
        {
            var testCode = @"";

            RoslynAssert.Valid(new PermittedTypeArgumentAnalyzer(), testCode);
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

            var diagnostic = ExpectedDiagnostic.Create(PermittedTypeArgumentAnalyzer.DiagnosticID);
            RoslynAssert.Diagnostics(new PermittedTypeArgumentAnalyzer(), diagnostic, testCode);
        }

        // Usage of prohibited type arguments for class
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

            var diagnostic = ExpectedDiagnostic.Create(PermittedTypeArgumentAnalyzer.DiagnosticID);
            RoslynAssert.Diagnostics(new PermittedTypeArgumentAnalyzer(), diagnostic, testCode);
        }
    }
}
