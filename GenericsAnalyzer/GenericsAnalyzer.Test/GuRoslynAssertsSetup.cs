using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GenericsAnalyzer.Test
{
    public static class GuRoslynAssertsSetup
    {
        [ModuleInitializer]
        public static void Setup()
        {
            Settings.Default = Settings.Default
                .WithAllowedCompilerDiagnostics(AllowedCompilerDiagnostics.WarningsAndErrors)
                .WithMetadataReferences(MetadataReferences.Transitive(typeof(GuRoslynAssertsSetup)));
        }
    }
}
