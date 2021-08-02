using GenericsAnalyzer.Core.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core
{
    public class TypeConstraintProfileInfoCollection
    {
        private readonly Dictionary<INamedTypeSymbol, TypeConstraintProfileInfo> profiles = new Dictionary<INamedTypeSymbol, TypeConstraintProfileInfo>();
        private readonly Dictionary<INamedTypeSymbol, TypeConstraintProfileGroupInfo> groups = new Dictionary<INamedTypeSymbol, TypeConstraintProfileGroupInfo>();

        public void AddProfile(INamedTypeSymbol profileDeclarationType, IEnumerable<INamedTypeSymbol> groupTypes)
        {
            profiles.Add(profileDeclarationType, new TypeConstraintProfileInfo(profileDeclarationType, groupTypes.Select(group => groups[group])));
        }
        public void AddGroup(INamedTypeSymbol groupDeclarationType, bool distinct)
        {
            groups.Add(groupDeclarationType, new TypeConstraintProfileGroupInfo(groupDeclarationType, distinct));
        }

        public bool ContainsProfile(INamedTypeSymbol profileDeclarationType) => profiles.ContainsKey(profileDeclarationType);
        public bool ContainsGroup(INamedTypeSymbol groupDeclarationType) => groups.ContainsKey(groupDeclarationType);

        public bool ContainsDeclaringType(INamedTypeSymbol declarationType) => ContainsProfile(declarationType) || ContainsGroup(declarationType);

        public TypeConstraintProfileInfo GetProfileInfo(INamedTypeSymbol profileDeclarationType) => profiles[profileDeclarationType];
        public TypeConstraintProfileGroupInfo GetGroupInfo(INamedTypeSymbol groupDeclarationType) => groups[groupDeclarationType];
    }

    public class TypeConstraintProfileGroupInfo
    {
        public INamedTypeSymbol GroupDeclaringInterface { get; }

        public bool Distinct { get; }

        public TypeConstraintProfileGroupInfo(INamedTypeSymbol groupDeclaringInterface, bool distinct)
        {
            GroupDeclaringInterface = groupDeclaringInterface;
            Distinct = distinct;
        }

        public override int GetHashCode() => SymbolEqualityComparer.Default.GetHashCode(GroupDeclaringInterface);
    }
    public class TypeConstraintProfileInfo
    {
        private readonly HashSet<TypeConstraintProfileGroupInfo> groups = new HashSet<TypeConstraintProfileGroupInfo>();

        public TypeConstraintSystem System { get; private set; }
        public TypeConstraintSystem.Builder Builder { get; }

        public IEnumerable<TypeConstraintProfileGroupInfo> Groups => groups.ToArray();

        public INamedTypeSymbol ProfileDeclaringInterface => Builder.ProfileInterface;

        public TypeConstraintProfileInfo(INamedTypeSymbol profileDeclaringInterface)
        {
            Builder = new TypeConstraintSystem.Builder(profileDeclaringInterface);
        }
        public TypeConstraintProfileInfo(INamedTypeSymbol profileDeclaringInterface, IEnumerable<TypeConstraintProfileGroupInfo> groupInfos)
            : this(profileDeclaringInterface)
        {
            AddToGroups(groupInfos);
        }

        public void AddToGroup(TypeConstraintProfileGroupInfo groupInfo) => groups.Add(groupInfo);
        public void AddToGroups(params TypeConstraintProfileGroupInfo[] groupInfos) => groups.AddRange(groupInfos);
        public void AddToGroups(IEnumerable<TypeConstraintProfileGroupInfo> groupInfos) => groups.AddRange(groupInfos);

        public void FinalizeSystem()
        {
            System = Builder.FinalizeSystem();
        }

        public override int GetHashCode() => SymbolEqualityComparer.Default.GetHashCode(ProfileDeclaringInterface);
    }
}
