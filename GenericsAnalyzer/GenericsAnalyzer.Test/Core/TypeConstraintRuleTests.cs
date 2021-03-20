using GenericsAnalyzer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.Core
{
    using static TypeConstraintRule;

    [TestClass]
    public class TypeConstraintRuleTests
    {
        [TestMethod]
        public void FullySatisfiesTest()
        {
            Assert.IsTrue(PermitBaseType.FullySatisfies(PermitBaseType));
            Assert.IsTrue(PermitBaseType.FullySatisfies(PermitExactType));
            Assert.IsTrue(ProhibitBaseType.FullySatisfies(ProhibitExactType));

            Assert.IsFalse(PermitExactType.FullySatisfies(PermitBaseType));
            Assert.IsFalse(ProhibitExactType.FullySatisfies(PermitBaseType));
            Assert.IsFalse(ProhibitExactType.FullySatisfies(PermitExactType));
            Assert.IsFalse(ProhibitExactType.FullySatisfies(ProhibitBaseType));
        }
    }
}
