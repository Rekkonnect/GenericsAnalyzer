using Microsoft.CodeAnalysis.Diagnostics;

namespace GenericsAnalyzer.Test
{
    public abstract class PermittedTypeArgumentAnalyzerDiagnosticTests : BaseDiagnosticTests
    {
        protected sealed override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new PermittedTypeArgumentAnalyzer();
    }
}
