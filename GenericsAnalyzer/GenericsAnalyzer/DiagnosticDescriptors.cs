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
            GA0001_Rule = GetDiagnosticDescriptor(0001, APIRestrictionsCategory, DiagnosticSeverity.Error),
            GA0002_Rule = GetDiagnosticDescriptor(0002, ValidityCategory, DiagnosticSeverity.Error),
            GA0003_Rule = GetDiagnosticDescriptor(0003, BrevityCategory, DiagnosticSeverity.Warning),
            GA0004_Rule = GetDiagnosticDescriptor(0004, ValidityCategory, DiagnosticSeverity.Error),
            GA0005_Rule = GetDiagnosticDescriptor(0005, ValidityCategory, DiagnosticSeverity.Error),
            GA0006_Rule = GetDiagnosticDescriptor(0006, BrevityCategory, DiagnosticSeverity.Warning),
            GA0008_Rule = GetDiagnosticDescriptor(0008, BrevityCategory, DiagnosticSeverity.Warning),
            GA0009_Rule = GetDiagnosticDescriptor(0009, BrevityCategory, DiagnosticSeverity.Warning),
            GA0010_Rule = GetDiagnosticDescriptor(0010, BrevityCategory, DiagnosticSeverity.Warning),
            GA0011_Rule = GetDiagnosticDescriptor(0011, BrevityCategory, DiagnosticSeverity.Warning),
            GA0012_Rule = GetDiagnosticDescriptor(0012, ValidityCategory, DiagnosticSeverity.Error),
            GA0013_Rule = GetDiagnosticDescriptor(0013, DesignCategory, DiagnosticSeverity.Warning),
            GA0014_Rule = GetDiagnosticDescriptor(0014, BrevityCategory, DiagnosticSeverity.Warning),
            GA0015_Rule = GetDiagnosticDescriptor(0015, BrevityCategory, DiagnosticSeverity.Warning),
            GA0016_Rule = GetDiagnosticDescriptor(0016, BrevityCategory, DiagnosticSeverity.Warning),
            GA0017_Rule = GetDiagnosticDescriptor(0017, APIRestrictionsCategory, DiagnosticSeverity.Error),
            GA0019_Rule = GetDiagnosticDescriptor(0019, ValidityCategory, DiagnosticSeverity.Error),
            GA0020_Rule = GetDiagnosticDescriptor(0020, ValidityCategory, DiagnosticSeverity.Error),
            GA0021_Rule = GetDiagnosticDescriptor(0021, ValidityCategory, DiagnosticSeverity.Error),
            GA0022_Rule = GetDiagnosticDescriptor(0022, ValidityCategory, DiagnosticSeverity.Error),
            GA0023_Rule = GetDiagnosticDescriptor(0023, ValidityCategory, DiagnosticSeverity.Error),
            GA0024_Rule = GetDiagnosticDescriptor(0024, DesignCategory, DiagnosticSeverity.Warning),
            GA0025_Rule = GetDiagnosticDescriptor(0025, DesignCategory, DiagnosticSeverity.Warning),
            GA0026_Rule = GetDiagnosticDescriptor(0026, ValidityCategory, DiagnosticSeverity.Error),
            GA0027_Rule = GetDiagnosticDescriptor(0027, ValidityCategory, DiagnosticSeverity.Error),
            GA0028_Rule = GetDiagnosticDescriptor(0028, APIRestrictionsCategory, DiagnosticSeverity.Error),
            GA0029_Rule = GetDiagnosticDescriptor(0029, ValidityCategory, DiagnosticSeverity.Error),
            GA0030_Rule = GetDiagnosticDescriptor(0030, DesignCategory, DiagnosticSeverity.Warning);
        #endregion

        private static readonly Dictionary<Type, HashSet<DiagnosticDescriptor>> analyzerGroupedDiagnostics = new Dictionary<Type, HashSet<DiagnosticDescriptor>>();
        private static readonly Dictionary<string, DiagnosticDescriptor> diagnosticsByID = new Dictionary<string, DiagnosticDescriptor>();

        static DiagnosticDescriptors()
        {
            int ruleIDLength = GetDiagnosticID(0).Length;

            var fields = typeof(DiagnosticDescriptors).GetFields();
            foreach (var field in fields)
            {
                // All rule fields must have the DiagnosticSupportedAttribute
                var type = field.GetCustomAttribute<DiagnosticSupportedAttribute>()?.DiagnosticAnalyzerType;
                if (type is null)
                    continue;

                var ruleID = field.Name.Substring(0, ruleIDLength);
                diagnosticsByID.Add(ruleID, field.GetValue(null) as DiagnosticDescriptor);

                if (!analyzerGroupedDiagnostics.TryGetValue(type, out var set))
                    analyzerGroupedDiagnostics.Add(type, set = new HashSet<DiagnosticDescriptor>());

                set.Add(field.GetValue(null) as DiagnosticDescriptor);
            }
        }

        public static DiagnosticDescriptor GetDiagnosticDescriptor(string ruleID)
        {
            diagnosticsByID.TryGetValue(ruleID, out var value);
            return value;
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

        private static LocalizableString GetTitle(int id) => GetResourceString(id, "Title");
        private static LocalizableString GetMessageFormat(int id) => GetResourceString(id, "MessageFormat");
        private static LocalizableString GetDescription(int id) => GetResourceString(id, "Description");
        private static LocalizableString GetResourceString(int id, string property)
        {
            return Resources.ResourceManager.GetString($"{GetDiagnosticID(id)}_{property}");
        }
        #endregion
    }
}
