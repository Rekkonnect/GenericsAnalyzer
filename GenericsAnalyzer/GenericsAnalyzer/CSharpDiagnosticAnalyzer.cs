using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using static GenericsAnalyzer.DiagnosticDescriptors;

namespace GenericsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public abstract class CSharpDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private static readonly IDictionary<Type, ImmutableArray<DiagnosticDescriptor>> groupedDiagnosticDescriptors = GetDiagnosticDescriptorsByAnalyzersImmutable();

        public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => groupedDiagnosticDescriptors[GetType()];
    }
}
