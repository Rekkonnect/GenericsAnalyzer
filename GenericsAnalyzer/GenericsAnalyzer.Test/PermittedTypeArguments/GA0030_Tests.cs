using GenericsAnalyzer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0030_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void NonProfileInterfaceWithProfileRelatedAttributes()
        {
            TestInterfaceWithProfileRelatedAttributes(null, true);
        }
        [TestMethod]
        public void ProfileGroupInterfaceWithProfileRelatedAttributes()
        {
            TestInterfaceWithProfileRelatedAttributes(nameof(TypeConstraintProfileGroupAttribute), true);
        }
        [TestMethod]
        public void ProfileInterfaceWithProfileRelatedAttributes()
        {
            TestInterfaceWithProfileRelatedAttributes(nameof(TypeConstraintProfileAttribute), false);
        }

        private void TestInterfaceWithProfileRelatedAttributes(string profileAttribute, bool assertDiagnostics)
        {
            if (!string.IsNullOrEmpty(profileAttribute))
                profileAttribute = $"[{profileAttribute}]";

            var testCode =
$@"
// Individually test every attribute
[↓PermittedTypes(typeof(int))]
{profileAttribute}
interface IA {{ }}
[↓ProhibitedTypes(typeof(long))]
{profileAttribute}
interface IB {{ }}
[↓PermittedBaseTypes(typeof(IEnumerable))]
{profileAttribute}
interface IC {{ }}
[↓ProhibitedBaseTypes(typeof(ICollection))]
{profileAttribute}
interface ID {{ }}
[↓OnlyPermitSpecifiedTypes]
{profileAttribute}
interface IE {{ }}
";

            AssertOrValidateWithUsings(testCode, assertDiagnostics);
        }
    }
}
