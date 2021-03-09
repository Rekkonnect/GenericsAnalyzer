using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    public sealed class TypeConstraintSystemAddResult
    {
        private readonly TypeConstraintSystemDiagnostics typeDiagnostics;

        public bool Success { get; }
        public ISet<ITypeSymbol> ConflictingTypes => typeDiagnostics.ConflictingTypes;
        public ISet<ITypeSymbol> DuplicateTypes => typeDiagnostics.DuplicateTypes;

        public TypeConstraintSystemAddResult(ISet<ITypeSymbol> conflictingTypeSet, ISet<ITypeSymbol> duplicateTypeSet)
        {
            Success = !conflictingTypeSet.Any();
            typeDiagnostics = new TypeConstraintSystemDiagnostics(conflictingTypeSet, duplicateTypeSet);
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
