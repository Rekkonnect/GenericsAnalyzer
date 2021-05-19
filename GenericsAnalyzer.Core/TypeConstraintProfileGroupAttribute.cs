using System;

namespace GenericsAnalyzer.Core
{
    /// <summary>Denotes that the marked interface represents a type constraint profile group.</summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class TypeConstraintProfileGroupAttribute : Attribute, IGenericTypeConstraintAttribute
    {
        /// <summary>Determines whether the profile group is a distinct one. Defaults to <see langword="true"/>.</summary>
        public bool Distinct { get; set; } = true;
    }
}
