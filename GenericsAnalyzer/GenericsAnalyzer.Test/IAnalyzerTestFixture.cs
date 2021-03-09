using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Test
{
    public interface IAnalyzerTestFixture
    {
        DiagnosticDescriptor TestedDiagnosticRule { get; }
    }
}
