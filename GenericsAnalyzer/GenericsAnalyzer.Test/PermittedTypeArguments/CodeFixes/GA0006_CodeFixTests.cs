using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsAnalyzer.Test.PermittedTypeArguments.CodeFixes
{
    [TestClass]
    public class GA0006_CodeFixTests : ConstraintClauseTypeConstraintPlacerCodeFixTests
    {
        public override DiagnosticDescriptor TestedDiagnosticRule => DiagnosticDescriptors.GA0006_Rule;

        [TestMethod]
        public void ReducibleConstraintBaseClass()
        {
            var testCode =
@"
class Test
<
    [PermittedBaseTypes({|GA0006:typeof(C)|})]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : A
{
}

class A { }
class B : A { }
class C : B { }
";

            var fixedCode =
@"
class Test
<
    T
>
    where T : C
{
}

class A { }
class B : A { }
class C : B { }
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintMultipleConstraintAttributes()
        {
            var testCode =
@"
class Test
<
    [ProhibitedTypes(typeof(C))]
    [PermittedBaseTypes({|GA0006:typeof(B)|})]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : A
{
}

class A { }
class B : A { }
class C : B { }
";

            var fixedCode =
@"
class Test
<
    [ProhibitedTypes(typeof(C))]
    T
>
    where T : B
{
}

class A { }
class B : A { }
class C : B { }
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintMoreTypePermissionAttributes()
        {
            var testCode =
@"
#pragma warning disable GA0010
#pragma warning disable GA0013

class Test
<
    [PermittedBaseTypes({|GA0006:typeof(IB)|})]
    [PermittedTypes(typeof(ID))]
    [ProhibitedBaseTypes(typeof(IC))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IA
{
}

interface IA { }
interface IB : IA { }
interface IC : IB { }
interface ID : IC { }
";

            var fixedCode =
@"
#pragma warning disable GA0010
#pragma warning disable GA0013

class Test
<
    [PermittedTypes(typeof(ID))]
    [ProhibitedBaseTypes(typeof(IC))]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : IA, IB
{
}

interface IA { }
interface IB : IA { }
interface IC : IB { }
interface ID : IC { }
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintInterfaceAndClass()
        {
            var testCode =
@"
class Test
<
    [PermittedBaseTypes({|GA0006:typeof(IC)|})]
    [OnlyPermitSpecifiedTypes]
    T
>
    where T : A
{
}

class A { }
class B : A { }
class C : B, IC { }
interface IC { }
";

            // The code fix should never freely replace A with C, only because it matches the one use case
            var fixedCode =
@"
class Test
<
    T
>
    where T : A, IC
{
}

class A { }
class B : A { }
class C : B, IC { }
interface IC { }
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintInterface()
        {
            var testCode =
@"
class C
<
    [PermittedBaseTypes({|GA0006:typeof(IComparable<int>)|})]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    T
> where T : IComparable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintInterfaceEnclosedWithinIrrelevantAttributes()
        {
            var testCode =
@"
class C
<
    [Example, PermittedBaseTypes({|GA0006:typeof(IComparable<int>)|}), Example]
    [OnlyPermitSpecifiedTypes]
    T
>
{
}
";

            var fixedCode =
@"
class C
<
    [Example, Example]
    T
> where T : IComparable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintMultipleTypeParameters()
        {
            var testCode =
@"
class C
<
    [Example, PermittedBaseTypes({|GA0006:typeof(IComparable<int>)|}), Example]
    [OnlyPermitSpecifiedTypes]
    T,
    [Example]
    U
>
    where U : IComparable<int>
{
}
";

            var fixedCode =
@"
class C
<
    [Example, Example]
    T,
    [Example]
    U
> where T : IComparable<int>
    where U : IComparable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
        [TestMethod]
        public void ReducibleConstraintMultipleClausedTypeParameters()
        {
            var testCode =
@"
class C
<
    R,
    S,
    [PermittedBaseTypes({|GA0006:typeof(IComparable<int>)|})]
    [OnlyPermitSpecifiedTypes]
    T,
    U,
    V,
    W
>
    where R : U
    where U : IComparable<int>
    where V : IComparable<int>
    where W : IComparable<int>
{
}
";

            var fixedCode =
@"
class C
<
    R,
    S,
    T,
    U,
    V,
    W
>
    where R : U
    where T : IComparable<int>
    where U : IComparable<int>
    where V : IComparable<int>
    where W : IComparable<int>
{
}
";

            TestCodeFixWithUsings(testCode, fixedCode);
        }
    }
}
