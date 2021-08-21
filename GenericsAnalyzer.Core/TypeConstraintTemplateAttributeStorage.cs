using Microsoft.CodeAnalysis;
using RoseLynn.Utilities;
using System;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core
{
    using TemplateType = TypeConstraintTemplateType;

    public class TypeConstraintTemplateAttributeStorage
    {
        private static readonly AssociatedPropertyContainer templateAssociativity = new AssociatedPropertyContainer(typeof(TypeConstraintTemplateAttributeStorage));

        [AssociatedTemplateType(TemplateType.Profile)]
        public AttributeData ProfileAttribute { get; set; }
        [AssociatedTemplateType(TemplateType.ProfileGroup)]
        public AttributeData ProfileGroupAttribute { get; set; }

        public AttributeData ProfileRelatedAttribute
        {
            get
            {
                if (ProfileAttribute != null && ProfileGroupAttribute != null)
                    throw new InvalidOperationException("There should only be one profile-related attribute.");

                return ProfileAttribute ?? ProfileGroupAttribute;
            }
        }

        public IEnumerable<AttributeData> GetAllAssociatedAttributes() => GetAssociatedAttributes(TemplateType.All);
        public IEnumerable<AttributeData> GetAssociatedAttributes(TemplateType templateTypes)
        {
            // Enums deserve much more love
            for (int mask = 1; mask < (int)TemplateType.All; mask <<= 1)
            {
                if (((int)templateTypes & mask) == default)
                    continue;

                var associatedAttribute = templateAssociativity.GetAssociatedProperty((TemplateType)mask).GetValue(this) as AttributeData;
                // yield return? x;
                if (associatedAttribute != null)
                    yield return associatedAttribute;
            }
            yield break;
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        private sealed class AssociatedTemplateTypeAttribute : Attribute, IAssociatedEnumValueAttribute<TemplateType>
        {
            public TemplateType AssociatedValue { get; }

            public AssociatedTemplateTypeAttribute(TemplateType templateType)
            {
                AssociatedValue = templateType;
            }
        }
    }
}
