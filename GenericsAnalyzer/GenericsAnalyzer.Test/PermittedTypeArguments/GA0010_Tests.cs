using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0010_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0010_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void RedundantProhibitions()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(typeof(int), typeof(short))]
    [ProhibitedTypes(↓typeof(long))]
    [ProhibitedBaseTypes(↓typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}

class D
<
    [PermittedTypes(typeof(List<>))]
    [ProhibitedTypes(typeof(List<int>))]
    [ProhibitedTypes(↓typeof(IList<int>))]
    [ProhibitedBaseTypes(↓typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void RedundantProhibitionsOnUnrestrictedPermission()
        {
            var testCode =
@"
class D
<
    [ProhibitedTypes(↓typeof(long))]
    [ProhibitedBaseTypes(typeof(IComparable<>))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void DeepInheritanceTree()
        {
            var testCode =
@"
class A
<
    [ProhibitedBaseTypes(typeof(IA), ↓typeof(IB), ↓typeof(IC))]
    [PermittedBaseTypes(typeof(ID))]
    [ProhibitedBaseTypes(typeof(IF))]
    [PermittedBaseTypes(typeof(IG))]
    [ProhibitedBaseTypes(typeof(II), ↓typeof(IJ))]
    T
>
{
}

interface IBase { }
interface IA : IBase { }
interface IB : IA { }
interface IC : IB { }
interface ID : IC { }
interface IE : ID { }
interface IF : IE { }
interface IG : IF { }
interface IH : IG { }
interface II : IH { }
interface IJ : II { }
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
