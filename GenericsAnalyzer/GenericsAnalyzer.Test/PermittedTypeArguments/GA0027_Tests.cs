using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0027_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void InvalidProfileGroupUsage()
        {
            var testCode =
$@"
class ClassA {{ }} 
struct StructA {{ }}
enum EnumA {{ }}
delegate void DelegateA();
record RecordA;

interface IIrrelevant {{ }}

[TypeConstraintProfile(↓typeof(void*), ↓typeof(ClassA), ↓typeof(StructA), ↓typeof(EnumA), ↓typeof(DelegateA), ↓typeof(RecordA), ↓typeof(IIrrelevant), typeof(IGroup0), typeof(IGroup1))]
interface IProfile {{ }}

[TypeConstraintProfileGroup(true)]
interface IGroup0 {{ }}
[TypeConstraintProfileGroup(false)]
interface IGroup1 {{ }}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
