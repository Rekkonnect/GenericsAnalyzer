using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    public sealed class TypeConstraintSystemDiagnostics
    {
        private readonly Dictionary<TypeConstraintSystemDiagnosticType, ISet<ITypeSymbol>> erroneousTypes = new Dictionary<TypeConstraintSystemDiagnosticType, ISet<ITypeSymbol>>
        {
            { TypeConstraintSystemDiagnosticType.Conflicting, NewSymbolHashSet() },
            { TypeConstraintSystemDiagnosticType.Duplicate, NewSymbolHashSet() },
            { TypeConstraintSystemDiagnosticType.InvalidTypeArgument, NewSymbolHashSet() },
            { TypeConstraintSystemDiagnosticType.ConstrainedTypeArgumentSubstitution, NewSymbolHashSet() },
        };

        private ISet<ITypeSymbol> ConflictingTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.Conflicting];
        private ISet<ITypeSymbol> DuplicateTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.Duplicate];
        private ISet<ITypeSymbol> InvalidTypeArgumentTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.InvalidTypeArgument];
        private ISet<ITypeSymbol> ConstrainedTypeArgumentSubstitutionTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.ConstrainedTypeArgumentSubstitution];

        public bool HasErroneousTypes
        {
            get
            {
                foreach (var set in erroneousTypes.Values)
                    if (set.Any())
                        return true;
                return false;
            }
        }

        public TypeConstraintSystemDiagnostics() { }
        public TypeConstraintSystemDiagnostics(TypeConstraintSystemDiagnostics other)
        {
            foreach (var kvp in other.erroneousTypes)
                erroneousTypes[kvp.Key].AddRange(kvp.Value);
        }

        public ISet<ITypeSymbol> GetTypesForDiagnosticType(TypeConstraintSystemDiagnosticType diagnosticType)
        {
            return new HashSet<ITypeSymbol>(erroneousTypes[diagnosticType], SymbolEqualityComparer.Default);
        }

        public void RegisterTypes(TypeConstraintSystemDiagnostics typeDiagnostics)
        {
            RegisterConflictingTypes(typeDiagnostics.ConflictingTypes);
            RegisterDuplicateTypes(typeDiagnostics.DuplicateTypes);
            InvalidTypeArgumentTypes.AddRange(typeDiagnostics.InvalidTypeArgumentTypes);
            ConstrainedTypeArgumentSubstitutionTypes.AddRange(typeDiagnostics.ConstrainedTypeArgumentSubstitutionTypes);
        }

        public void RegisterConflictingType(ITypeSymbol type)
        {
            if (DuplicateTypes.Contains(type))
                DuplicateTypes.Remove(type);

            ConflictingTypes.Add(type);
        }
        public void RegisterDuplicateType(ITypeSymbol type)
        {
            DuplicateTypes.Add(type);
        }
        public bool ConditionallyRegisterInvalidTypeArgumentType(ITypeSymbol type)
        {
            bool invalid = type.IsInvalidTypeArgument();
            if (invalid)
                InvalidTypeArgumentTypes.Add(type);
            return invalid;
        }
        public bool ConditionallyRegisterConstrainedSubstitutionType(ITypeParameterSymbol typeParameter, ITypeSymbol type)
        {
            bool invalid = !typeParameter.IsValidTypeArgumentSubstitution(type);
            if (invalid)
                ConstrainedTypeArgumentSubstitutionTypes.Add(type);
            return invalid;
        }

        public void RegisterConflictingTypes(ISet<ITypeSymbol> types)
        {
            // Syntax idea
            // RegisterConflictingType foreach in types;

            foreach (var type in types)
                RegisterConflictingType(type);
        }
        public void RegisterDuplicateTypes(ISet<ITypeSymbol> types)
        {
            DuplicateTypes.AddRange(types);
        }

        public TypeConstraintSystemDiagnosticType GetDiagnosticType(ITypeSymbol type)
        {
            if (ConflictingTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.Conflicting;
            if (DuplicateTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.Duplicate;
            if (InvalidTypeArgumentTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.InvalidTypeArgument;
            if (ConstrainedTypeArgumentSubstitutionTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.ConstrainedTypeArgumentSubstitution;
            return TypeConstraintSystemDiagnosticType.Valid;
        }

        private static HashSet<ITypeSymbol> NewSymbolHashSet() => new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
    }
}
