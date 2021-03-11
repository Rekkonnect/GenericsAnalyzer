using System;

namespace GenericsAnalyzer.Core
{
    public struct TypeConstraintRule : IEquatable<TypeConstraintRule>
    {
        public TypeConstraintReferencePoint TypeReferencePoint { get; set; }
        public ConstraintRule Rule { get; set; }

        public TypeConstraintRule(TypeConstraintReferencePoint applicability, ConstraintRule rule)
        {
            TypeReferencePoint = applicability;
            Rule = rule;
        }
        public TypeConstraintRule(TypeConstraintRule other)
            : this(other.TypeReferencePoint, other.Rule) { }

        public static bool operator ==(TypeConstraintRule left, TypeConstraintRule right) => left.Equals(right);
        public static bool operator !=(TypeConstraintRule left, TypeConstraintRule right) => !left.Equals(right);

        public bool Equals(TypeConstraintRule other) => TypeReferencePoint == other.TypeReferencePoint && Rule == other.Rule;
        public override bool Equals(object obj) => obj is TypeConstraintRule rule && Equals(rule);

        public override int GetHashCode()
        {
            return TypeReferencePoint.GetHashCode() | (Rule.GetHashCode() << (sizeof(TypeConstraintReferencePoint) * 8));
        }
        public override string ToString() => $"{Rule} {TypeReferencePoint}";
    }
}
