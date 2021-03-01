using GenericsAnalyzer.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace GenericsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PermittedTypeArgumentAnalyzer : DiagnosticAnalyzer
    {
        #region Analyzer Metadata
        public const string DiagnosticID = "GA0001";
        public const string Category = "API Restrictions";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.GA0001_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.GA0001_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.GA0001_Description), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticID, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        #endregion 

        private readonly GenericTypeConstraintInfoCollection genericNames = new GenericTypeConstraintInfoCollection();
        private readonly GenericNameUsageCollection genericTypeUsages = new GenericNameUsageCollection();

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            // Concurrent execution is disabled due to the stateful profile of the analyzer

            context.RegisterSyntaxNodeAction(AnalyzeGenericName, SyntaxKind.GenericName);
        }

        private void AnalyzeGenericName(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var genericNameNode = context.Node as GenericNameSyntax;
            if (genericNameNode.IsUnboundGenericName)
                return;

            var symbolInfo = semanticModel.GetSymbolInfo(genericNameNode);
            var symbol = symbolInfo.Symbol;

            if (symbol.IsDefinition)
                return;

            genericTypeUsages.Register(symbol, genericNameNode);
            var originalDefinition = symbol.OriginalDefinition;
            AnalyzeGenericTypeDefinition(originalDefinition);
            AnalyzeGenericTypeUsage(context, symbol, genericNameNode);
        }

        private void AnalyzeGenericTypeDefinition(ISymbol symbol)
        {
            if (genericNames.ContainsInfo(symbol))
                return;

            ImmutableArray<ITypeParameterSymbol> typeParameters;

            // This is in dire need of some better abstraction
            // Say something like IGenericSupportSymbol
            if (symbol is INamedTypeSymbol t)
                typeParameters = t.TypeParameters;
            else if (symbol is IMethodSymbol m)
                typeParameters = m.TypeParameters;
            else
                return;

            var constraints = new GenericTypeConstraintInfo(typeParameters.Length);
            for (int i = 0; i < typeParameters.Length; i++)
            {
                var parameter = typeParameters[i];

                var attributes = parameter.GetAttributes();
                var system = new TypeConstraintSystem();
                foreach (var a in attributes)
                {
                    var rule = ParseAttributeRule(a);
                    if (rule is null)
                        continue;

                    system.Add(rule.Value, a.ConstructorArguments[0].Values.Select(c => c.Value as INamedTypeSymbol));
                }

                constraints[i] = system;
            }

            genericNames[symbol] = constraints;
        }
        private void AnalyzeGenericTypeUsage(SyntaxNodeAnalysisContext context, ISymbol symbol, GenericNameSyntax genericNameNode)
        {
            var semanticModel = context.SemanticModel;

            var originalDefinition = symbol.OriginalDefinition;

            var typeArguments = genericNameNode.TypeArgumentList.Arguments.ToArray();

            var constraints = genericNames[originalDefinition];
            for (int i = 0; i < typeArguments.Length; i++)
            {
                var argument = typeArguments[i];

                var system = constraints[i];
                var argumentType = semanticModel.GetTypeInfo(argument);
                if (!system.IsPermitted(argumentType.Type as INamedTypeSymbol))
                {
                    var diagnostic = Diagnostic.Create(Rule, argument.GetLocation(), originalDefinition.ToDisplayString(), argumentType.Type.ToDisplayString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static TypeConstraintRule? ParseAttributeRule(AttributeData data)
        {
            if (!ConstrainedTypesAttributeBase.ConstrainedTypeAttributeTypes.Any(t => t.Name == data.AttributeClass.Name))
                return null;

            return ConstrainedTypesAttributeBase.GetConstraintRule(data.AttributeClass.Name);
        }
    }
}
