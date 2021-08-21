using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0005_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void InvalidTypeArguments()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(string))]
    [PermittedBaseTypes(typeof(IEnumerable<>))]
    [PermittedBaseTypes(↓typeof(List<>))]
    [PermittedTypes(typeof(IList<int>), typeof(ISet<int>))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IEnumerable<int>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void StructConstraint()
        {
            var testCode =
@"
class C { }
struct S { }
struct Managed
{
    List<int> list;
}

class Generic
<
    [PermittedTypes(↓typeof(string))]
    [ProhibitedTypes(↓typeof(IEnumerable<int>))]
    [ProhibitedBaseTypes(typeof(IEnumerable<uint>))]
    [ProhibitedTypes(typeof(int))]
    [ProhibitedTypes(typeof(Managed))]
    [PermittedBaseTypes(↓typeof(C))]
    T
>
    where T : struct
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void UnmanagedConstraint()
        {
            var testCode =
@"
class C { }
struct S { }
struct Managed
{
    List<int> list;
}

class Generic
<
    [PermittedTypes(↓typeof(string))]
    [ProhibitedTypes(↓typeof(IEnumerable<int>))]
    [ProhibitedBaseTypes(typeof(IEnumerable<uint>))]
    [ProhibitedTypes(typeof(int))]
    [ProhibitedTypes(↓typeof(Managed))]
    [PermittedBaseTypes(↓typeof(C))]
    T
>
    where T : unmanaged
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void ClassConstraint()
        {
            var testCode =
@"
class C { }
struct S { }
struct Managed
{
    List<int> list;
}

class Generic
<
    [PermittedTypes(typeof(string))]
    [ProhibitedTypes(typeof(IEnumerable<int>))]
    [ProhibitedBaseTypes(typeof(IEnumerable<uint>))]
    [ProhibitedTypes(↓typeof(int))]
    [ProhibitedTypes(↓typeof(Managed))]
    [PermittedBaseTypes(typeof(C))]
    T
>
    where T : class
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void NewConstraint()
        {
            var testCode =
@"
class C { }
struct S { }
struct Managed
{
    List<int> list;
}

class Generic
<
    [PermittedTypes(↓typeof(string))]
    [ProhibitedTypes(↓typeof(IEnumerable<int>))]
    [ProhibitedBaseTypes(typeof(IEnumerable<uint>))]
    [ProhibitedTypes(typeof(int))]
    [ProhibitedTypes(typeof(Managed))]
    [PermittedBaseTypes(typeof(C))]
    T
>
    where T : new()
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
