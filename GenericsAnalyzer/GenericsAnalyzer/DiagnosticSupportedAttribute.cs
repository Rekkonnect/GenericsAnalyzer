using System;

namespace GenericsAnalyzer
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DiagnosticSupportedAttribute : Attribute
    {
        public Type DiagnosticAnalyzerType { get; }

        public DiagnosticSupportedAttribute(Type diagnosticAnalyzerType) => DiagnosticAnalyzerType = diagnosticAnalyzerType;
    }
}
