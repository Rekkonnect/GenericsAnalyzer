using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GenericsAnalyzer.Core
{
    public sealed class TypeConstraintSystemDiagnostics
    {
        private readonly ISet<ITypeSymbol> conflictingTypes;
        private readonly ISet<ITypeSymbol> duplicateTypes;

        public ISet<ITypeSymbol> ConflictingTypes => new HashSet<ITypeSymbol>(conflictingTypes, SymbolEqualityComparer.Default);
        public ISet<ITypeSymbol> DuplicateTypes => new HashSet<ITypeSymbol>(duplicateTypes, SymbolEqualityComparer.Default);

        public TypeConstraintSystemDiagnostics()
            : this(new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default), new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default)) { }

        public TypeConstraintSystemDiagnostics(ISet<ITypeSymbol> conflictingTypeSet, ISet<ITypeSymbol> duplicateTypeSet)
        {
            conflictingTypes = conflictingTypeSet;
            duplicateTypes = duplicateTypeSet;
        }

        public void RegisterTypes(TypeConstraintSystemDiagnostics typeDiagnostics)
        {
            RegisterTypes(typeDiagnostics.conflictingTypes, typeDiagnostics.duplicateTypes);
        }
        public void RegisterTypes(ISet<ITypeSymbol> conflictingTypeSet, ISet<ITypeSymbol> duplicateTypeSet)
        {
            RegisterConflictingTypes(conflictingTypeSet);
            RegisterDuplicateTypes(duplicateTypeSet);
        }

        public void RegisterConflictingTypes(ISet<ITypeSymbol> types)
        {
            foreach (var type in types)
            {
                if (duplicateTypes.Contains(type))
                    duplicateTypes.Remove(type);

                conflictingTypes.Add(type);
            }
        }
        public void RegisterDuplicateTypes(ISet<ITypeSymbol> types)
        {
            foreach (var type in types)
                duplicateTypes.Add(type);
        }

        public TypeConstraintSystemDiagnosticType GetDiagnosticType(ITypeSymbol type)
        {
            if (conflictingTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.Conflicting;
            if (duplicateTypes.Contains(type))
                return TypeConstraintSystemDiagnosticType.Duplicate;
            return TypeConstraintSystemDiagnosticType.Valid;
        }
    }
}
