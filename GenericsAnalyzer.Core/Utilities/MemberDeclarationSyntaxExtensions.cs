using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class MemberDeclarationSyntaxExtensions
    {
        // Again, should be abstracted into something like GenericMemberDeclarationSyntax
        public static int GetArity(this MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case TypeDeclarationSyntax t:
                    return t.Arity;
                case DelegateDeclarationSyntax d:
                    return d.Arity;
                case MethodDeclarationSyntax m:
                    return m.Arity;
            }
            return 0;
        }
        public static bool IsGeneric(this MemberDeclarationSyntax syntax) => syntax.GetArity() > 0;

        public static TypeParameterListSyntax GetTypeParameterList(this MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case TypeDeclarationSyntax t:
                    return t.TypeParameterList;
                case DelegateDeclarationSyntax d:
                    return d.TypeParameterList;
                case MethodDeclarationSyntax m:
                    return m.TypeParameterList;
            }
            return null;
        }
    }
}
