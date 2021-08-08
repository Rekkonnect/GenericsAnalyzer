using System;

namespace GenericsAnalyzer.Core
{
    [AttributeUsage(AttributeTargets.GenericParameter, AllowMultiple = false)]
    public abstract class BaseInheritConstraintsAttribute : Attribute, IGenericTypeConstraintAttribute
    {
    }
}
