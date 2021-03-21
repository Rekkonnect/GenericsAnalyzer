using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace GenericsAnalyzer.Core.Utilities
{
    public class TypeParameterConstraintClauseSegmentation
    {
        private TypeParameterConstraintSyntax keywordOrClassConstraint;
        private TypeConstraintSyntax delegateOrEnumConstraint;
        private List<TypeConstraintSyntax> interfaceConstraints = new List<TypeConstraintSyntax>();
        private ConstructorConstraintSyntax newConstraint;

        private SemanticModel semanticModel;

        public TypeParameterConstraintClauseSegmentation(TypeParameterConstraintClauseSyntax constraintClause, SemanticModel semanticModel)
            : this(constraintClause.Constraints, semanticModel) { }
        public TypeParameterConstraintClauseSegmentation(IEnumerable<TypeParameterConstraintSyntax> constraints, SemanticModel model)
        {
            semanticModel = model;
            Add(constraints);
        }

        private void Add(IEnumerable<TypeParameterConstraintSyntax> constraints)
        {
            foreach (var c in constraints)
                AddInitializationConstraint(c);
        }
        private void AddInitializationConstraint(TypeParameterConstraintSyntax constraint)
        {
            switch (constraint)
            {
                case TypeConstraintSyntax typeConstraint:
                    var type = typeConstraint.Type;
                    var typeSymbol = semanticModel.GetTypeInfo(type).Type;
                    AddTypeConstraint(typeConstraint, typeSymbol);
                    break;

                case DefaultConstraintSyntax _:
                case ClassOrStructConstraintSyntax _:
                    keywordOrClassConstraint = constraint;
                    break;

                case ConstructorConstraintSyntax constructorConstraint:
                    newConstraint = constructorConstraint;
                    break;
            }
        }

        public void AddTypeConstraint(TypeConstraintSyntax typeConstraint, ITypeSymbol typeSymbol)
        {
            switch (typeSymbol.TypeKind)
            {
                case TypeKind.Class:
                    switch (typeSymbol.SpecialType)
                    {
                        case SpecialType.System_Delegate:
                        case SpecialType.System_Enum:
                            delegateOrEnumConstraint = typeConstraint;
                            return;
                        default:
                            keywordOrClassConstraint = typeConstraint;
                            return;
                    }
                case TypeKind.Interface:
                    interfaceConstraints.Add(typeConstraint);
                    break;
            }
        }

        public TypeParameterConstraintClauseSyntax WithTheseConstraints(TypeParameterConstraintClauseSyntax constraintClause)
        {
            return constraintClause.WithConstraints(ToSeparatedSyntaxList()).WithTriviaFrom(constraintClause);
        }
        public SeparatedSyntaxList<TypeParameterConstraintSyntax> ToSeparatedSyntaxList()
        {
            var result = new SeparatedSyntaxList<TypeParameterConstraintSyntax>();
            if (keywordOrClassConstraint != null)
                result = result.Add(keywordOrClassConstraint);
            if (delegateOrEnumConstraint != null)
                result = result.Add(delegateOrEnumConstraint);
            if (interfaceConstraints.Any())
                result = result.AddRange(interfaceConstraints);
            if (newConstraint != null)
                result = result.Add(newConstraint);

            return result;
        }
    }
}
