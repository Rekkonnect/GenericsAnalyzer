using Microsoft.CodeAnalysis;
using RoseLynn.Analyzers;
using System.Resources;

namespace GenericsAnalyzer
{
    internal sealed class GADiagnosticDescriptorStorage : DiagnosticDescriptorStorageBase
    {
        public static readonly GADiagnosticDescriptorStorage Instance = new();

        protected override string BaseRuleDocsURI => "https://github.com/AlFasGD/GenericsAnalyzer/blob/master/docs/rules";
        protected override string DiagnosticIDPrefix => "GA";
        protected override ResourceManager ResourceManager => Resources.ResourceManager;

        #region Category Constants
        public const string APIRestrictionsCategory = "API Restrictions";
        public const string BrevityCategory = "Brevity";
        public const string DesignCategory = "Design";
        public const string InformationCategory = "Information";
        public const string ValidityCategory = "Validity";
        #endregion

        #region Rules
        [DiagnosticSupported(typeof(PermittedTypeArgumentAnalyzer))]
        public readonly DiagnosticDescriptor
            GA0001_Rule,
            GA0002_Rule,
            GA0003_Rule,
            GA0004_Rule,
            GA0005_Rule,
            GA0006_Rule,
            GA0008_Rule,
            GA0009_Rule,
            GA0010_Rule,
            GA0011_Rule,
            GA0012_Rule,
            GA0013_Rule,
            GA0014_Rule,
            GA0015_Rule,
            GA0016_Rule,
            GA0017_Rule,
            GA0019_Rule,
            GA0020_Rule,
            GA0021_Rule,
            GA0022_Rule,
            GA0023_Rule,
            GA0024_Rule,
            GA0025_Rule,
            GA0026_Rule,
            GA0027_Rule,
            GA0028_Rule,
            GA0029_Rule,
            GA0030_Rule;

        private GADiagnosticDescriptorStorage()
        {
            GA0001_Rule = CreateDiagnosticDescriptor(0001, APIRestrictionsCategory, DiagnosticSeverity.Error);
            GA0002_Rule = CreateDiagnosticDescriptor(0002, ValidityCategory, DiagnosticSeverity.Error);
            GA0003_Rule = CreateDiagnosticDescriptor(0003, BrevityCategory, DiagnosticSeverity.Warning);
            GA0004_Rule = CreateDiagnosticDescriptor(0004, ValidityCategory, DiagnosticSeverity.Error);
            GA0005_Rule = CreateDiagnosticDescriptor(0005, ValidityCategory, DiagnosticSeverity.Error);
            GA0006_Rule = CreateDiagnosticDescriptor(0006, BrevityCategory, DiagnosticSeverity.Warning);
            GA0008_Rule = CreateDiagnosticDescriptor(0008, BrevityCategory, DiagnosticSeverity.Warning);
            GA0009_Rule = CreateDiagnosticDescriptor(0009, BrevityCategory, DiagnosticSeverity.Warning);
            GA0010_Rule = CreateDiagnosticDescriptor(0010, BrevityCategory, DiagnosticSeverity.Warning);
            GA0011_Rule = CreateDiagnosticDescriptor(0011, BrevityCategory, DiagnosticSeverity.Warning);
            GA0012_Rule = CreateDiagnosticDescriptor(0012, ValidityCategory, DiagnosticSeverity.Error);
            GA0013_Rule = CreateDiagnosticDescriptor(0013, DesignCategory, DiagnosticSeverity.Warning);
            GA0014_Rule = CreateDiagnosticDescriptor(0014, BrevityCategory, DiagnosticSeverity.Warning);
            GA0015_Rule = CreateDiagnosticDescriptor(0015, BrevityCategory, DiagnosticSeverity.Warning);
            GA0016_Rule = CreateDiagnosticDescriptor(0016, BrevityCategory, DiagnosticSeverity.Warning);
            GA0017_Rule = CreateDiagnosticDescriptor(0017, APIRestrictionsCategory, DiagnosticSeverity.Error);
            GA0019_Rule = CreateDiagnosticDescriptor(0019, ValidityCategory, DiagnosticSeverity.Error);
            GA0020_Rule = CreateDiagnosticDescriptor(0020, ValidityCategory, DiagnosticSeverity.Error);
            GA0021_Rule = CreateDiagnosticDescriptor(0021, ValidityCategory, DiagnosticSeverity.Error);
            GA0022_Rule = CreateDiagnosticDescriptor(0022, ValidityCategory, DiagnosticSeverity.Error);
            GA0023_Rule = CreateDiagnosticDescriptor(0023, ValidityCategory, DiagnosticSeverity.Error);
            GA0024_Rule = CreateDiagnosticDescriptor(0024, DesignCategory, DiagnosticSeverity.Warning);
            GA0025_Rule = CreateDiagnosticDescriptor(0025, DesignCategory, DiagnosticSeverity.Warning);
            GA0026_Rule = CreateDiagnosticDescriptor(0026, ValidityCategory, DiagnosticSeverity.Error);
            GA0027_Rule = CreateDiagnosticDescriptor(0027, ValidityCategory, DiagnosticSeverity.Error);
            GA0028_Rule = CreateDiagnosticDescriptor(0028, APIRestrictionsCategory, DiagnosticSeverity.Error);
            GA0029_Rule = CreateDiagnosticDescriptor(0029, ValidityCategory, DiagnosticSeverity.Error);
            GA0030_Rule = CreateDiagnosticDescriptor(0030, DesignCategory, DiagnosticSeverity.Warning);
        }
        #endregion
    }
}
