﻿using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments
{
    [TestClass]
    public sealed class GA0003_Tests : PermittedTypeArgumentAnalyzerDiagnosticTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0003_Rule;

        [TestMethod]
        public void BoundUnbound()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(IComparable<int>), typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void BoundUnboundDifferentAttributes()
        {
            var testCode =
@"
class C
<
    [PermittedTypes(↓typeof(IComparable<int>))]
    [PermittedTypes(typeof(IComparable<>))]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}
