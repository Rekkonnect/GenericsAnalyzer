﻿using System;

namespace GenericsAnalyzer.Core
{
    /// <summary>Denotes that the marked generic type parameter constraints may be inherited from usages in base types.</summary>
    [AttributeUsage(AttributeTargets.GenericParameter, AllowMultiple = false)]
    public sealed class InheritBaseTypeUsageConstraintsAttribute : Attribute, IGenericTypeConstraintAttribute
    {
    }
}
