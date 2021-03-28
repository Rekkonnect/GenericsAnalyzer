using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    /// <summary>Denotes that a generic type argument permits the usage of the specified types.</summary>
    [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class ConstrainedTypesAttributeBase : Attribute, IGenericTypeConstraintAttribute
    {
        private static Type[] constrainedTypeAttributeTypes;
        private static Dictionary<Type, ConstrainedTypesAttributeBase> defaultInstances;

        /// <summary>Gets all the constrained type attribute types that exist in this assembly.</summary>
        public static Type[] ConstrainedTypeAttributeTypes => constrainedTypeAttributeTypes.ToArray();

        static ConstrainedTypesAttributeBase()
        {
            var baseType = typeof(ConstrainedTypesAttributeBase);
            constrainedTypeAttributeTypes = baseType.Assembly.GetTypes().Where(IsValidConstraintRuleAttributeType).ToArray();

            defaultInstances = new Dictionary<Type, ConstrainedTypesAttributeBase>(constrainedTypeAttributeTypes.Length);
            foreach (var type in constrainedTypeAttributeTypes)
            {
                var instance = (ConstrainedTypesAttributeBase)type.GetConstructor(new[] { typeof(Type[]) }).Invoke(new[] { Type.EmptyTypes } );
                defaultInstances.Add(type, instance);
            }
        }

        private Type[] types;

        protected abstract TypeConstraintRule Rule { get; }

        /// <summary>Gets the types that are permitted.</summary>
        public Type[] Types => types.ToArray();

        /// <summary>Initializes a new instance of the <seealso cref="ConstrainedTypesAttributeBase"/> from the given permitted types.</summary>
        /// <param name="constrainedTypes">The types that are constrained accordingly for the marked generic type.</param>
        protected ConstrainedTypesAttributeBase(params Type[] constrainedTypes)
        {
            types = constrainedTypes;
        }

        public static TypeConstraintRule GetConstraintRule<T>()
            where T : ConstrainedTypesAttributeBase
        {
            return GetConstraintRule(typeof(T));
        }
        public static TypeConstraintRule GetConstraintRule(string typeName)
        {
            return GetConstraintRule(constrainedTypeAttributeTypes.FirstOrDefault(t => t.Name == typeName));
        }
        public static TypeConstraintRule GetConstraintRule(Type type)
        {
            if (!IsValidConstraintRuleAttributeType(type))
                throw new ArgumentException($"The type must be a non-abstract type that inherits from {nameof(ConstrainedTypesAttributeBase)}");

            return defaultInstances[type].Rule;
        }

        public static bool IsValidConstraintRuleAttributeType(Type type)
        {
            var baseType = typeof(ConstrainedTypesAttributeBase);
            return !type.IsAbstract && baseType.IsAssignableFrom(type);
        }
    }
}
