﻿using System;

namespace GenericsAnalyzer.Core
{
    using static TypeConstraintRule;

    /// <summary>Denotes that a generic type argument prohibits the usage of the specified types and the types' inheritors. That means, if a type inherits one of the base types that are provided in this attribute, it is prohibited too.</summary>
    public class ProhibitedBaseTypesAttribute : ConstrainedTypesAttributeBase
    {
        protected override TypeConstraintRule Rule => ProhibitBaseType;

        /// <summary>Initializes a new instance of the <seealso cref="ProhibitedBaseTypesAttribute"/> from the given prohibited types.</summary>
        /// <param name="prohibitedBaseTypes">The base types that are prohibited as a generic type argument for the marked generic type.</param>
        public ProhibitedBaseTypesAttribute(params Type[] prohibitedBaseTypes)
            : base(prohibitedBaseTypes) { }
    }
}
