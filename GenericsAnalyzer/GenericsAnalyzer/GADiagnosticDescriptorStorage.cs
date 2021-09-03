﻿using Microsoft.CodeAnalysis;
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
        private GADiagnosticDescriptorStorage()
        {
            SetDefaultDiagnosticAnalyzer<PermittedTypeArgumentAnalyzer>();

            CreateDiagnosticDescriptor(0001, APIRestrictionsCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0002, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0003, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0004, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0005, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0006, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0008, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0009, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0010, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0011, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0012, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0013, DesignCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0014, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0015, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0016, BrevityCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0017, APIRestrictionsCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0019, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0020, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0021, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0022, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0023, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0024, DesignCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0025, DesignCategory, DiagnosticSeverity.Warning);
            CreateDiagnosticDescriptor(0026, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0027, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0028, APIRestrictionsCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0029, ValidityCategory, DiagnosticSeverity.Error);
            CreateDiagnosticDescriptor(0030, DesignCategory, DiagnosticSeverity.Warning);
        }
        #endregion
    }
}
