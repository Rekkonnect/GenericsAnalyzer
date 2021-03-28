using System;

namespace GenericsAnalyzer.Core
{
    /// <summary>Denotes that the marked interface represents a type constraint profile.</summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class TypeConstraintProfileGroupAttribute : Attribute, IGenericTypeConstraintAttribute
    {
        /// <summary>Determines whether the profile group is a distinct one.</summary>
        public bool Distinct { get; set; } = true;
    }
}
