using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0011_Tests : BaseDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0011_Rule;

        protected override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();

        [TestMethod]
        public void RedundantPermissions()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int), ↓typeof(short))]
    [ProhibitedTypes(typeof(long))]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void RedundantPermissionsOnRestrictedPermissions()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(int), ↓typeof(short))]
    [PermittedBaseTypes(typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
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
    [ProhibitedBaseTypes(typeof(IA))]
    [PermittedBaseTypes(typeof(ID), ↓typeof(IE))]
    [ProhibitedBaseTypes(typeof(IF))]
    [PermittedBaseTypes(typeof(IG), ↓typeof(IH))]
    [ProhibitedBaseTypes(typeof(II))]
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
