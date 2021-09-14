﻿using RoseLynn.Utilities;
using System;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    /// <summary>Denotes that a generic type argument permits the usage of the specified types.</summary>
    [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class ConstrainedTypesAttributeBase : Attribute, IGenericTypeConstraintAttribute
    {
        private static readonly InstanceContainer instanceContainer = new();

        private sealed class InstanceContainer : DefaultInstanceContainer<ConstrainedTypesAttributeBase>
        {
            protected override object[] GetDefaultInstanceArguments()
            {
                return new object[] { Type.EmptyTypes };
            }
        }

        private readonly Type[] types;

        protected abstract TypeConstraintRule Rule { get; }

        /// <summary>Gets the types that are permitted.</summary>
        public Type[] Types => types.ToArray();

        /// <summary>Initializes a new instance of the <seealso cref="ConstrainedTypesAttributeBase"/> from the given permitted types.</summary>
        /// <param name="constrainedTypes">The types that are constrained accordingly for the marked generic type.</param>
        protected ConstrainedTypesAttributeBase(params Type[] constrainedTypes)
        {
            types = constrainedTypes;
        }

        public static TypeConstraintRule? GetConstraintRule<T>()
            where T : ConstrainedTypesAttributeBase
        {
            return instanceContainer.GetDefaultInstance<T>()?.Rule;
        }
        /// <summary>Gets the constraint rule that the attribute with the given attribute name reflects.</summary>
        /// <param name="attributeTypeName">The name of the attribute whose constraint rule to get.</param>
        /// <returns>The <seealso cref="TypeConstraintRule"/> that is reflected from the attribute with the given name.</returns>
        public static TypeConstraintRule? GetConstraintRuleFromAttributeName(string attributeTypeName)
        {
            return instanceContainer.GetDefaultInstance(attributeTypeName)?.Rule;
        }
        public static TypeConstraintRule? GetConstraintRule(Type type)
        {
            return instanceContainer.GetDefaultInstance(type)?.Rule;
        }
    }
}
