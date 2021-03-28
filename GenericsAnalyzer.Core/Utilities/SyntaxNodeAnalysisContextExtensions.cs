using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class SyntaxNodeAnalysisContextExtensions
    {
        public static void ReportDiagnostics<T>(this SyntaxNodeAnalysisContext context, IEnumerable<T> nodes, Func<T, Diagnostic> diagnosticGenerator)
            where T : SyntaxNode
        {
            foreach (var n in nodes)
                context.ReportDiagnostic(diagnosticGenerator(n));
        }
    }
}
