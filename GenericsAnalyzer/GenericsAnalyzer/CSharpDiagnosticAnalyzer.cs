using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public abstract class CSharpDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => GetDiagnosticDescriptors(GetType()).ToImmutableArray();
    }
}
