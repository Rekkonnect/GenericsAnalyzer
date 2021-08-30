using GenericsAnalyzer.Test;
using Gu.Roslyn.Asserts;

// Properly includes all the required assemblies for the compiled code in the tests
[assembly: TransitiveMetadataReferences
(
    // All assemblies that this assembly depends on
    typeof(BaseGADiagnosticTests<>)
)]