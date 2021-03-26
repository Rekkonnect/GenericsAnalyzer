using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace GenericsAnalyzer
{
    internal static class DiagnosticDescriptors
    {
        #region Category Constants
        public const string APIRestrictionsCategory = "API Restrictions";
        public const string BrevityCategory = "Brevity";
        public const string DesignCategory = "Design";
        public const string InformationCategory = "Information";
        public const string ValidityCategory = "Validity";
        #endregion

        #region Rules
        [DiagnosticSupported(typeof(PermittedTypeArgumentAnalyzer))]
        public static readonly DiagnosticDescriptor
            GA0001_Rule = GetDiagnosticDescriptor(1, APIRestrictionsCategory, DiagnosticSeverity.Error),
            GA0002_Rule = GetDiagnosticDescriptor(2, ValidityCategory, DiagnosticSeverity.Error),
            GA0003_Rule = GetDiagnosticDescriptor(3, BrevityCategory, DiagnosticSeverity.Warning),
            GA0004_Rule = GetDiagnosticDescriptor(4, ValidityCategory, DiagnosticSeverity.Error),
            GA0005_Rule = GetDiagnosticDescriptor(5, ValidityCategory, DiagnosticSeverity.Error),
            GA0006_Rule = GetDiagnosticDescriptor(6, BrevityCategory, DiagnosticSeverity.Warning),
            GA0008_Rule = GetDiagnosticDescriptor(8, BrevityCategory, DiagnosticSeverity.Warning),
            GA0009_Rule = GetDiagnosticDescriptor(9, BrevityCategory, DiagnosticSeverity.Warning),
            GA0010_Rule = GetDiagnosticDescriptor(10, BrevityCategory, DiagnosticSeverity.Warning),
            GA0011_Rule = GetDiagnosticDescriptor(11, BrevityCategory, DiagnosticSeverity.Warning),
            GA0012_Rule = GetDiagnosticDescriptor(12, ValidityCategory, DiagnosticSeverity.Error),
            GA0013_Rule = GetDiagnosticDescriptor(13, DesignCategory, DiagnosticSeverity.Warning),
            GA0014_Rule = GetDiagnosticDescriptor(14, BrevityCategory, DiagnosticSeverity.Warning),
            GA0015_Rule = GetDiagnosticDescriptor(15, BrevityCategory, DiagnosticSeverity.Warning),
            GA0016_Rule = GetDiagnosticDescriptor(16, BrevityCategory, DiagnosticSeverity.Warning),
            GA0017_Rule = GetDiagnosticDescriptor(17, APIRestrictionsCategory, DiagnosticSeverity.Error),
            GA0019_Rule = GetDiagnosticDescriptor(19, ValidityCategory, DiagnosticSeverity.Error),
            GA0020_Rule = GetDiagnosticDescriptor(20, ValidityCategory, DiagnosticSeverity.Error),
            GA0021_Rule = GetDiagnosticDescriptor(21, ValidityCategory, DiagnosticSeverity.Error),
            GA0022_Rule = GetDiagnosticDescriptor(22, ValidityCategory, DiagnosticSeverity.Error);
        #endregion

        private static readonly Dictionary<Type, HashSet<DiagnosticDescriptor>> analyzerGroupedDiagnostics = new Dictionary<Type, HashSet<DiagnosticDescriptor>>();

        static DiagnosticDescriptors()
        {
            var fields = typeof(DiagnosticDescriptors).GetFields();
            foreach (var f in fields)
            {
                var type = f.GetCustomAttribute<DiagnosticSupportedAttribute>()?.DiagnosticAnalyzerType;
                if (type is null)
                    continue;

                if (!analyzerGroupedDiagnostics.TryGetValue(type, out var set))
                    analyzerGroupedDiagnostics.Add(type, set = new HashSet<DiagnosticDescriptor>());

                set.Add(f.GetValue(null) as DiagnosticDescriptor);
            }
        }

        public static IDictionary<Type, ImmutableArray<DiagnosticDescriptor>> GetDiagnosticDescriptorsByAnalyzersImmutable()
        {
            return analyzerGroupedDiagnostics.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToImmutableArray());
        }
        public static ISet<DiagnosticDescriptor> GetDiagnosticDescriptors(Type diagnosticAnalyzerType)
        {
            analyzerGroupedDiagnostics.TryGetValue(diagnosticAnalyzerType, out var set);
            return new HashSet<DiagnosticDescriptor>(set);
        }

        #region Diagnotsic Descriptor Construction
        private const string baseRuleDocsURL = "https://github.com/AlFasGD/GenericsAnalyzer/blob/master/docs/rules";

        private static string GetHelpLinkURI(int id) => $"{baseRuleDocsURL}/{GetDiagnosticID(id)}.md";
        private static string GetDiagnosticID(int id) => $"GA{id:0000}";

        private static DiagnosticDescriptor GetDiagnosticDescriptor(int id, string category, DiagnosticSeverity severity)
        {
            return new DiagnosticDescriptor(GetDiagnosticID(id), GetTitle(id), GetMessageFormat(id), category, severity, true, helpLinkUri: GetHelpLinkURI(id), description: GetDescription(id));
        }

        private static LocalizableResourceString GetTitle(int id) => GetResourceString(id, "Title");
        private static LocalizableResourceString GetMessageFormat(int id) => GetResourceString(id, "MessageFormat");
        private static LocalizableResourceString GetDescription(int id) => GetResourceString(id, "Description");
        private static LocalizableResourceString GetResourceString(int id, string property)
        {
            try
            {
                return new LocalizableResourceString($"{GetDiagnosticID(id)}_{property}", Resources.ResourceManager, typeof(Resources));
            }
            catch { }

            return null;
        }
        #endregion
    }
}
