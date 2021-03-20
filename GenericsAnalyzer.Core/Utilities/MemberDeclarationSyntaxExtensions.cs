using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class MemberDeclarationSyntaxExtensions
    {
        public static bool IsGeneric(this MemberDeclarationSyntax syntax) => syntax.GetArity() > 0;

        // This is just gross
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
        public static SyntaxList<TypeParameterConstraintClauseSyntax> GetConstraintClauses(this MemberDeclarationSyntax syntax)
        {
            switch (syntax)
            {
                case TypeDeclarationSyntax t:
                    return t.ConstraintClauses;
                case DelegateDeclarationSyntax d:
                    return d.ConstraintClauses;
                case MethodDeclarationSyntax m:
                    return m.ConstraintClauses;
            }
            return default;
        }
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

        public static MemberDeclarationSyntax WithConstraintClauses(this MemberDeclarationSyntax syntax, SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            switch (syntax)
            {
                case TypeDeclarationSyntax t:
                    return t.WithConstraintClauses(constraintClauses);
                case DelegateDeclarationSyntax d:
                    return d.WithConstraintClauses(constraintClauses);
                case MethodDeclarationSyntax m:
                    return m.WithConstraintClauses(constraintClauses);
            }
            return null;
        }
    }
}
