using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Core
{
    public sealed class TypeConstraintSystemAddResult
    {
        private readonly TypeConstraintSystemDiagnostics typeDiagnostics;

        public bool Success => !typeDiagnostics.HasErroneousTypes;

        public TypeConstraintSystemAddResult(TypeConstraintSystemDiagnostics diagnostics)
        {
            typeDiagnostics = new TypeConstraintSystemDiagnostics(diagnostics);
        }

        public void RegisterOnto(TypeConstraintSystemDiagnostics diagnostics)
        {
            if (Success)
                return;

            diagnostics.RegisterTypes(typeDiagnostics);
        }

        public TypeConstraintSystemDiagnosticType GetDiagnosticType(ITypeSymbol type) => typeDiagnostics.GetDiagnosticType(type);
    }
}
