using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core
{
    public class GenericTypeConstraintInfo
    {
        private readonly TypeConstraintSystem[] systems;

        public GenericTypeConstraintInfo(int parameterCount)
        {
            systems = new TypeConstraintSystem[parameterCount];
        }

        public bool IsPermitted(int parameterIndex, INamedTypeSymbol type)
        {
            return systems[parameterIndex].IsPermitted(type);
        }

        public TypeConstraintSystem this[int index]
        {
            get => systems[index];
            set => systems[index] = value;
        }
    }
}
