using GenericsAnalyzer.Core;
using GenericsAnalyzer.Test.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using RoslynTestKit;

namespace GenericsAnalyzer.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

                    // Metadata references - currently the only reason to use RoslynTestKit
                    solution = solution.AddMetadataReferences(projectId, new[]
                    {
                        ReferenceSource.FromAssembly(typeof(IGenericTypeConstraintAttribute).Assembly),
                        ReferenceSource.FromAssembly(typeof(ExampleAttribute).Assembly),
                    });

                    return solution;
                });
            }
        }
    }
}
