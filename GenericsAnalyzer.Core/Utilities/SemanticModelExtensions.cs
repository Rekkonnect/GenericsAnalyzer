﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenericsAnalyzer.Core.Utilities
{
    public static class SemanticModelExtensions
    {
        public static TypeInfo GetTypeInfo(this SemanticModel semanticModel, BaseTypeSyntax baseType) => semanticModel.GetTypeInfo(baseType.Type);
    }
}
