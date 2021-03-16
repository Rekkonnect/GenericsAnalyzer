using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class ITypeParameterSymbolExtensions
    {
        public static ISymbol GetDeclaringSymbol(this ITypeParameterSymbol symbol)
        {
            return (ISymbol)symbol.DeclaringType ?? symbol.DeclaringMethod;
        }

        public static bool IsValidTypeArgumentSubstitution(this ITypeParameterSymbol symbol, ITypeSymbol typeArgument)
        {
            if (symbol.HasConstructorConstraint)
            {
                if (!(typeArgument is INamedTypeSymbol namedTypeArgument))
                    return false;

                if (!namedTypeArgument.InstanceConstructors.Any(c => c.Parameters.IsEmpty))
                    return false;
            }

            if (symbol.HasReferenceTypeConstraint)
                if (!typeArgument.IsReferenceType)
                    return false;

            if (symbol.HasUnmanagedTypeConstraint)
                if (!typeArgument.IsUnmanagedType)
                    return false;

            if (symbol.HasValueTypeConstraint)
                if (!typeArgument.IsValueType)
                    return false;

            if (symbol.HasNotNullConstraint)
                if (!typeArgument.IsNotNull())
                    return false;

            var set = new HashSet<ITypeSymbol>(typeArgument.GetAllBaseTypesAndInterfaces(), SymbolEqualityComparer.Default);
            foreach (var constraintType in symbol.ConstraintTypes)
            {
                if (constraintType is ITypeParameterSymbol typeParameterConstraint)
                {
                    // This can only check for whether the provided type meets the other type parameter's constraint;
                    // but cannot check whether it can be valid in context. The check is atomic to each type parameter
                    // per call of this function, each type argument substitution should mind the context within itself
                    // for type argument validity.
                    if (!IsValidTypeArgumentSubstitution(typeParameterConstraint, typeArgument))
                        return false;
                }
                else if (!set.Contains(constraintType))
                    return false;
            }
            
            return true;
        }
    }
}
