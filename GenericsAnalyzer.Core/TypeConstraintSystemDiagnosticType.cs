namespace GenericsAnalyzer.Core
{
    public enum TypeConstraintSystemDiagnosticType
    {
        Valid,
        Conflicting,
        Duplicate,
        InvalidTypeArgument,
        ConstrainedTypeArgumentSubstitution,
        RedundantlyPermitted,
        RedundantlyProhibited,
        ReducableToConstraintClause,
    }
}
