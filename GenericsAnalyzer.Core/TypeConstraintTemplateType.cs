using System;

namespace GenericsAnalyzer.Core
{
    [Flags]
    public enum TypeConstraintTemplateType
    {
        None,

        Profile = 1,
        ProfileGroup = 1 << 1,

        All = Profile | ProfileGroup
    }
}
