using System;

namespace GenericsAnalyzer.Core.Utilities
{
    public interface IAssociatedEnumValueAttribute<T>
        where T : struct, Enum
    {
        T AssociatedValue { get; }
    }
}
