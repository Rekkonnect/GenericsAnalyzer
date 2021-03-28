namespace GenericsAnalyzer.Core
{
    using BuildState = TypeConstraintSystem.Builder.SystemBuildState;

    // Because we can't have enum functions within the enums themselves
    public static class SystemBuildStateExtensions
    {
        public static bool HasFinalizedBase(this BuildState state) => state >= BuildState.FinalizedBase;
        public static bool HasFinalizedWhole(this BuildState state) => state >= BuildState.FinalizedWhole;
    }
}
