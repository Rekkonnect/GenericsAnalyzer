﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.CodeFixes.Test.PermittedTypeArguments
{
    [TestClass]
    public class GA0016_CodeFixTests : RedundantAttributeRemoverCodeFixTests
    {
        [TestMethod]
        public void RedundantUsageWithCodeFix()
        {
            var testCode =
@"
class A<T> { }
class C
<
    [{|*:InheritBaseTypeUsageConstraints|}]
    T
>
    : A<int>
{
}
";

            var fixedCode =
@"
class A<T> { }
class C
<
    T
>
    : A<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
