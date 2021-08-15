using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core
{
    public class TypeConstraintTemplateAttributeStorage
    {
        // TODO: Overengineer this system so that:
        //       - properties are accessed through reflection
        //       - association system is generalized
        [AssociatedTemplateType(TypeConstraintTemplateType.Profile)]
        public AttributeData ProfileAttribute { get; set; }
        [AssociatedTemplateType(TypeConstraintTemplateType.ProfileGroup)]
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

        public IEnumerable<AttributeData> GetAllAssociatedAttributes() => GetAssociatedAttributes(TypeConstraintTemplateType.All);
        public IEnumerable<AttributeData> GetAssociatedAttributes(TypeConstraintTemplateType templateTypes)
        {
            // yield return? x;
            if ((templateTypes & TypeConstraintTemplateType.Profile) != default)
                if (ProfileAttribute != null)
                    yield return ProfileAttribute;

            if ((templateTypes & TypeConstraintTemplateType.ProfileGroup) != default)
                if (ProfileGroupAttribute != null)
                    yield return ProfileGroupAttribute;
        }

        private sealed class AssociatedTemplateTypeAttribute : Attribute
        {
            public TypeConstraintTemplateType TemplateType { get; }

            public AssociatedTemplateTypeAttribute(TypeConstraintTemplateType templateType)
            {
                TemplateType = templateType;
            }
        }
    }
}
