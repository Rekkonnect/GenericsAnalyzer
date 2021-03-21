using System;

namespace GenericsAnalyzer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DiagnosticSupportedAttribute : Attribute
    {
        public Type DiagnosticAnalyzerType { get; }

        public DiagnosticSupportedAttribute(Type diagnosticAnalyzerType) => DiagnosticAnalyzerType = diagnosticAnalyzerType;
    }
}
