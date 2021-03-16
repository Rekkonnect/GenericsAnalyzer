using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    public sealed class TypeConstraintSystemDiagnostics
    {
        private static readonly TypeConstraintSystemDiagnosticType[] diagnosticTypes;

        static TypeConstraintSystemDiagnostics()
        {
            diagnosticTypes = Enum.GetValues(typeof(TypeConstraintSystemDiagnosticType)).Cast<TypeConstraintSystemDiagnosticType>().ToArray();
        }

        private readonly Dictionary<TypeConstraintSystemDiagnosticType, ISet<ITypeSymbol>> erroneousTypes = new Dictionary<TypeConstraintSystemDiagnosticType, ISet<ITypeSymbol>>();

        private ISet<ITypeSymbol> ConflictingTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.Conflicting];
        private ISet<ITypeSymbol> DuplicateTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.Duplicate];
        private ISet<ITypeSymbol> InvalidTypeArgumentTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.InvalidTypeArgument];
        private ISet<ITypeSymbol> ConstrainedTypeArgumentSubstitutionTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.ConstrainedTypeArgumentSubstitution];
        private ISet<ITypeSymbol> RedundantlyPermittedTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.RedundantlyPermitted];
        private ISet<ITypeSymbol> RedundantlyProhibitedTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.RedundantlyProhibited];
        private ISet<ITypeSymbol> ReducableToConstraintClauseTypes => erroneousTypes[TypeConstraintSystemDiagnosticType.ReducableToConstraintClause];

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

        public TypeConstraintSystemDiagnostics()
        {
            foreach (var type in diagnosticTypes)
                if (type != TypeConstraintSystemDiagnosticType.Valid)
                    erroneousTypes.Add(type, NewSymbolHashSet());
        }
        public TypeConstraintSystemDiagnostics(TypeConstraintSystemDiagnostics other)
            : this()
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
            RegisterDuplicateTypes(typeDiagnostics.DuplicateTypes);
            RegisterConflictingTypes(typeDiagnostics.ConflictingTypes);

            foreach (var kvp in typeDiagnostics.erroneousTypes)
            {
                // This should avoid directly adding elements that were previously handled
                switch (kvp.Key)
                {
                    case TypeConstraintSystemDiagnosticType.Conflicting:
                    case TypeConstraintSystemDiagnosticType.Duplicate:
                        continue;
                }

                erroneousTypes[kvp.Key].AddRange(kvp.Value);
            }
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
        public void RegisterReducableToConstraintClauseType(INamedTypeSymbol type) => ReducableToConstraintClauseTypes.Add(type);
        public void RegisterRedundantlyConstrainedType(ITypeSymbol type, ConstraintRule rule) => erroneousTypes[GetDiagnosticType(rule)].Add(type);
        public void RegisterRedundantlyPermittedType(ITypeSymbol type) => RedundantlyPermittedTypes.Add(type);
        public void RegisterRedundantlyProhibitedType(ITypeSymbol type) => RedundantlyProhibitedTypes.Add(type);

        public void RegisterConflictingTypes(ISet<ITypeSymbol> types)
        {
            foreach (var type in types)
                RegisterConflictingType(type);
        }
        public void RegisterDuplicateTypes(ISet<ITypeSymbol> types)
        {
            DuplicateTypes.AddRange(types);
        }

        public TypeConstraintSystemDiagnosticType GetDiagnosticType(ITypeSymbol type)
        {
            // There is no need to check for the key's value because Valid is the default value
            return erroneousTypes.FirstOrDefault(kvp => kvp.Value.Contains(type)).Key;
        }

        private static HashSet<ITypeSymbol> NewSymbolHashSet() => new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

        private static TypeConstraintSystemDiagnosticType GetDiagnosticType(ConstraintRule rule)
        {
            switch (rule)
            {
                case ConstraintRule.Permit:
                    return TypeConstraintSystemDiagnosticType.RedundantlyPermitted;
                case ConstraintRule.Prohibit:
                    return TypeConstraintSystemDiagnosticType.RedundantlyProhibited;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
