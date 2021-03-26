using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    using DiagnosticType = TypeConstraintSystemDiagnosticType;
    using InheritanceDiagnosticType = TypeConstraintSystemInheritanceDiagnosticType;

    public sealed class TypeConstraintSystemDiagnostics
    {
        private static readonly DiagnosticType[] diagnosticTypes;
        private static readonly InheritanceDiagnosticType[] inheritanceDiagnosticTypes;

        static TypeConstraintSystemDiagnostics()
        {
            diagnosticTypes = EnumHelpers.GetValues<DiagnosticType>();
            inheritanceDiagnosticTypes = EnumHelpers.GetValues<InheritanceDiagnosticType>();
        }

        private readonly ErroneousElementDictionary<DiagnosticType, ITypeSymbol> erroneousTypes;
        private readonly ErroneousElementDictionary<InheritanceDiagnosticType, ITypeParameterSymbol> erroneousInheritedTypeParameters;

        // DiagnosticType
        private ISet<ITypeSymbol> ConflictingTypes => erroneousTypes[DiagnosticType.Conflicting];
        private ISet<ITypeSymbol> DuplicateTypes => erroneousTypes[DiagnosticType.Duplicate];
        private ISet<ITypeSymbol> InvalidTypeArgumentTypes => erroneousTypes[DiagnosticType.InvalidTypeArgument];
        private ISet<ITypeSymbol> ConstrainedTypeArgumentSubstitutionTypes => erroneousTypes[DiagnosticType.ConstrainedTypeArgumentSubstitution];
        private ISet<ITypeSymbol> RedundantlyPermittedTypes => erroneousTypes[DiagnosticType.RedundantlyPermitted];
        private ISet<ITypeSymbol> RedundantlyProhibitedTypes => erroneousTypes[DiagnosticType.RedundantlyProhibited];
        private ISet<ITypeSymbol> ReducibleToConstraintClauseTypes => erroneousTypes[DiagnosticType.ReducibleToConstraintClause];
        private ISet<ITypeSymbol> RedundantBaseTypeRuleTypes => erroneousTypes[DiagnosticType.RedundantBaseTypeRule];
        private ISet<ITypeSymbol> RedundantBoundUnboundRuleTypes => erroneousTypes[DiagnosticType.RedundantBoundUnboundRule];

        // InheritanceDiagnosticType
        private ISet<ITypeParameterSymbol> ConflictingInheritedTypeParameters => erroneousInheritedTypeParameters[InheritanceDiagnosticType.Conflicting];

        public bool HasErroneousTypes => IEnumerableExtensions.AnyDeep(erroneousTypes.Values);
        public bool HasErroneousInheritedTypeParameters => IEnumerableExtensions.AnyDeep(erroneousInheritedTypeParameters.Values);

        public TypeConstraintSystemDiagnostics()
        {
            // Very long
            erroneousTypes = new ErroneousElementDictionary<DiagnosticType, ITypeSymbol>();
            foreach (var type in diagnosticTypes)
                if (type != default)
                    erroneousTypes.Add(type, NewSymbolHashSet<ITypeSymbol>());

            erroneousInheritedTypeParameters = new ErroneousElementDictionary<InheritanceDiagnosticType, ITypeParameterSymbol>();
            foreach (var type in inheritanceDiagnosticTypes)
                if (type != default)
                    erroneousInheritedTypeParameters.Add(type, NewSymbolHashSet<ITypeParameterSymbol>());
        }
        public TypeConstraintSystemDiagnostics(TypeConstraintSystemDiagnostics other)
            : this()
        {
            foreach (var kvp in other.erroneousTypes)
                erroneousTypes[kvp.Key].AddRange(kvp.Value);

            foreach (var kvp in other.erroneousInheritedTypeParameters)
                erroneousInheritedTypeParameters[kvp.Key].AddRange(kvp.Value);
        }

        public ISet<ITypeSymbol> GetTypesForDiagnosticType(DiagnosticType diagnosticType)
        {
            return new HashSet<ITypeSymbol>(erroneousTypes[diagnosticType], SymbolEqualityComparer.Default);
        }
        public ISet<ITypeParameterSymbol> GetTypeParametersForInheritanceDiagnosticType(InheritanceDiagnosticType diagnosticType)
        {
            return new HashSet<ITypeParameterSymbol>(erroneousInheritedTypeParameters[diagnosticType], SymbolEqualityComparer.Default);
        }

        public void RegisterDiagnostics(TypeConstraintSystemDiagnostics typeDiagnostics)
        {
            RegisterDuplicateTypes(typeDiagnostics.DuplicateTypes);
            RegisterConflictingTypes(typeDiagnostics.ConflictingTypes);

            foreach (var kvp in typeDiagnostics.erroneousTypes)
            {
                // This should avoid directly adding elements that were previously handled
                switch (kvp.Key)
                {
                    case DiagnosticType.Conflicting:
                    case DiagnosticType.Duplicate:
                        continue;
                }

                erroneousTypes[kvp.Key].AddRange(kvp.Value);
            }

            foreach (var kvp in typeDiagnostics.erroneousInheritedTypeParameters)
            {
                erroneousInheritedTypeParameters[kvp.Key].AddRange(kvp.Value);
            }
        }

        #region Register Type Diagnostics
        // Talk about a clusterfuck
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
        public bool ConditionallyRegisterConstrainedSubstitutionType(ITypeParameterSymbol typeParameter, ITypeSymbol type, bool evaluateAsBase)
        {
            bool invalid = !typeParameter.IsValidTypeArgumentSubstitution(type, evaluateAsBase);
            if (invalid)
                ConstrainedTypeArgumentSubstitutionTypes.Add(type);
            return invalid;
        }
        public bool ConditionallyRegisterRedundantBaseTypeRuleType(ITypeSymbol type, TypeConstraintRule constraintRule)
        {
            bool redundant = constraintRule.TypeReferencePoint is TypeConstraintReferencePoint.BaseType && type.IsSealed;
            if (redundant)
                RedundantBaseTypeRuleTypes.Add(type);
            return redundant;
        }
        public void RegisterReducibleToConstraintClauseType(INamedTypeSymbol type) => ReducibleToConstraintClauseTypes.Add(type);
        public void RegisterRedundantlyConstrainedType(ITypeSymbol type, ConstraintRule rule) => erroneousTypes[GetDiagnosticType(rule)].Add(type);
        public void RegisterRedundantlyPermittedType(ITypeSymbol type) => RedundantlyPermittedTypes.Add(type);
        public void RegisterRedundantlyProhibitedType(ITypeSymbol type) => RedundantlyProhibitedTypes.Add(type);
        public void RegisterRedundantBoundUnboundRuleType(INamedTypeSymbol type)
        {
            RedundantlyPermittedTypes.Remove(type);
            RedundantlyProhibitedTypes.Remove(type);
            RedundantBoundUnboundRuleTypes.Add(type);
        }

        public void RegisterConflictingTypes(ISet<ITypeSymbol> types)
        {
            foreach (var type in types)
                RegisterConflictingType(type);
        }
        public void RegisterDuplicateTypes(ISet<ITypeSymbol> types)
        {
            DuplicateTypes.AddRange(types);
        }
        #endregion

        #region Register Inherited Type Parameter Diagnostics
        public void RegisterConflictingInheritedTypeParameter(ITypeParameterSymbol typeParameter)
        {
            ConflictingInheritedTypeParameters.Add(typeParameter);
        }
        #endregion

        public DiagnosticType GetDiagnosticType(ITypeSymbol type)
        {
            return erroneousTypes.GetDiagnosticType(type);
        }
        public InheritanceDiagnosticType GetInheritanceDiagnosticType(ITypeParameterSymbol type)
        {
            return erroneousInheritedTypeParameters.GetDiagnosticType(type);
        }

        private static HashSet<T> NewSymbolHashSet<T>()
            where T : class, ISymbol
        {
            return new HashSet<T>(comparer: SymbolEqualityComparer.Default);
        }

        private static DiagnosticType GetDiagnosticType(ConstraintRule rule)
        {
            switch (rule)
            {
                case ConstraintRule.Permit:
                    return DiagnosticType.RedundantlyPermitted;
                case ConstraintRule.Prohibit:
                    return DiagnosticType.RedundantlyProhibited;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        // Aliasing is not possible just yet
        private class ErroneousElementDictionary<TDiagnosticType, TElement> : Dictionary<TDiagnosticType, ISet<TElement>>
            where TDiagnosticType : struct, Enum
        {
            // There is no need to check for the key's value because the valid value is the default value
            public TDiagnosticType GetDiagnosticType(TElement type) => this.FirstOrDefault(kvp => kvp.Value.Contains(type)).Key;
        }
    }
}
