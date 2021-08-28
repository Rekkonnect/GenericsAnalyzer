using RoseLynn.CodeFixes;
using System.Resources;

namespace GenericsAnalyzer
{
    public abstract class GACodeFixProvider : MultipleDiagnosticCodeFixProvider
    {
        protected sealed override ResourceManager ResourceManager => CodeFixResources.ResourceManager;
    }
}
