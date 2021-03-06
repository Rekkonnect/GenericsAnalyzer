using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    /// <summary>Represents a system that contains a set of rules about type constraints.</summary>
    public class TypeConstraintSystem
    {
        private Dictionary<ITypeSymbol, TypeConstraintRule> typeConstraintRules = new Dictionary<ITypeSymbol, TypeConstraintRule>(SymbolEqualityComparer.Default);

        private HashSet<ITypeParameterSymbol> inheritedTypes = new HashSet<ITypeParameterSymbol>(SymbolEqualityComparer.Default);

        public bool OnlyPermitSpecifiedTypes { get; set; }

        public void InheritFrom(ITypeParameterSymbol baseTypeParameter, TypeConstraintSystem baseSystem)
        {
            OnlyPermitSpecifiedTypes |= baseSystem.OnlyPermitSpecifiedTypes;
            typeConstraintRules.AddOrSetRange(baseSystem.typeConstraintRules);
            inheritedTypes.Add(baseTypeParameter);
        }

        public void Add(TypeConstraintRule rule, params ITypeSymbol[] types) => Add(rule, (IEnumerable<ITypeSymbol>)types);
        public void Add(TypeConstraintRule rule, IEnumerable<ITypeSymbol> types)
        {
            foreach (var t in types)
            {
                if (typeConstraintRules.ContainsKey(t))
                    typeConstraintRules[t] = rule;
                else
                    typeConstraintRules.Add(t, rule);
            }
        }

        public bool SupersetOf(TypeConstraintSystem other) => other.SubsetOf(this);
        public bool SubsetOf(TypeConstraintSystem other)
        {
            if (OnlyPermitSpecifiedTypes)
                if (!other.OnlyPermitSpecifiedTypes)
                    return false;

            foreach (var rule in other.typeConstraintRules)
                if (IsPermitted(rule.Key) != (rule.Value.Rule is ConstraintRule.Permit))
                    return false;

            return true;
        }

        public bool IsPermitted(ITypeParameterSymbol typeParameter, int typeParameterDeclarationIndex, GenericTypeConstraintInfoCollection infos)
        {
            if (inheritedTypes.Contains(typeParameter))
                return true;

            var declaringElementTypeParameterSystems = infos[(ISymbol)typeParameter.DeclaringType ?? typeParameter.DeclaringMethod];
            var system = declaringElementTypeParameterSystems[typeParameterDeclarationIndex];
            return SubsetOf(system);
        }

        public bool IsPermitted(ITypeSymbol type)
        {
            if (type is null)
                return false;

            var permission = IsPermitted(type, TypeConstraintReferencePoint.ExactType);
            if (permission != PermissionResult.Unknown)
                return permission == PermissionResult.Permitted;

            var interfaceQueue = new Queue<INamedTypeSymbol>(type.Interfaces);
            while (interfaceQueue.Any())
            {
                var i = interfaceQueue.Dequeue();

                permission = IsPermitted(i, TypeConstraintReferencePoint.BaseType);
                if (permission != PermissionResult.Unknown)
                    return permission == PermissionResult.Permitted;

                if (i.IsGenericType && !i.IsUnboundGenericType)
                {
                    var unbound = i.ConstructUnboundGenericType();
                    permission = IsPermitted(unbound, TypeConstraintReferencePoint.BaseType);
                    if (permission != PermissionResult.Unknown)
                        return permission == PermissionResult.Permitted;
                }

                foreach (var indirectInterface in i.Interfaces)
                    interfaceQueue.Enqueue(indirectInterface);
            }

            do
            {
                permission = IsPermitted(type, TypeConstraintReferencePoint.BaseType);
                if (permission != PermissionResult.Unknown)
                    return permission == PermissionResult.Permitted;

                type = type.BaseType;
            }
            while (!(type is null));

            return !OnlyPermitSpecifiedTypes;
        }

        private PermissionResult IsPermitted(ITypeSymbol type, TypeConstraintReferencePoint referencePoint)
        {
            if (!typeConstraintRules.ContainsKey(type))
                return PermissionResult.Unknown;

            var rule = typeConstraintRules[type];
            if (rule.TypeReferencePoint == referencePoint)
                return (PermissionResult)rule.Rule;

            return PermissionResult.Unknown;
        }
    }
}
