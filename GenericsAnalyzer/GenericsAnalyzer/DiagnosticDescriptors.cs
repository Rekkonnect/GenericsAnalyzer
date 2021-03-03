using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer
{
    internal static class DiagnosticDescriptors
    {
        public const string APIRestrictionsCategory = "API Restrictions";
        public const string BrevityCategory = "Brevity";

        #region Rules
        public static readonly DiagnosticDescriptor GA0001_Rule = GetDiagnosticDescriptor(1, APIRestrictionsCategory, DiagnosticSeverity.Error);
        public static readonly DiagnosticDescriptor GA0014_Rule = GetDiagnosticDescriptor(14, BrevityCategory, DiagnosticSeverity.Warning);
        #endregion

        public static string GetDiagnosticID(int id) => $"GA{id:0000}";

        private static DiagnosticDescriptor GetDiagnosticDescriptor(int id, string category, DiagnosticSeverity severity)
        {
            return new DiagnosticDescriptor(GetDiagnosticID(id), GetTitle(id), GetMessageFormat(id), category, severity, true, description: GetDescription(id));
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
    }
}
