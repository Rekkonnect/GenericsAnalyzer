using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer
{
    internal static class DiagnosticDescriptors
    {
        #region Category Constants
        public const string APIRestrictionsCategory = "API Restrictions";
        public const string BrevityCategory = "Brevity";
        public const string DesignCategory = "Design";
        public const string ValidityCategory = "Validity";
        #endregion

        #region Rules
        public static readonly DiagnosticDescriptor GA0001_Rule = GetDiagnosticDescriptor(1, APIRestrictionsCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0002_Rule = GetDiagnosticDescriptor(2, ValidityCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0003_Rule = GetDiagnosticDescriptor(3, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0004_Rule = GetDiagnosticDescriptor(4, ValidityCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0005_Rule = GetDiagnosticDescriptor(5, ValidityCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0006_Rule = GetDiagnosticDescriptor(6, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0007_Rule = GetDiagnosticDescriptor(7, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0008_Rule = GetDiagnosticDescriptor(8, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0009_Rule = GetDiagnosticDescriptor(9, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0010_Rule = GetDiagnosticDescriptor(10, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0011_Rule = GetDiagnosticDescriptor(11, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0012_Rule = GetDiagnosticDescriptor(12, ValidityCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0013_Rule = GetDiagnosticDescriptor(13, DesignCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0014_Rule = GetDiagnosticDescriptor(14, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0015_Rule = GetDiagnosticDescriptor(15, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0016_Rule = GetDiagnosticDescriptor(16, BrevityCategory, DiagnosticSeverity.Warning);
        public static readonly DiagnosticDescriptor GA0017_Rule = GetDiagnosticDescriptor(17, APIRestrictionsCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0018_Rule = GetDiagnosticDescriptor(18, BrevityCategory, DiagnosticSeverity.Warning);
        #endregion

        #region Diagnotsic Descriptor Construction
        private const string baseRuleDocsURL = "https://github.com/AlFasGD/GenericsAnalyzer/blob/master/docs/rules";

        public static string GetHelpLinkURI(int id) => $"{baseRuleDocsURL}/{GetDiagnosticID(id)}.md";
        public static string GetDiagnosticID(int id) => $"GA{id:0000}";

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
