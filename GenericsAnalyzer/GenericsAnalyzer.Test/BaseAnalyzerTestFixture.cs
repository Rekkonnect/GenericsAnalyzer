using Microsoft.CodeAnalysis;

namespace GenericsAnalyzer.Test
{
    public class BaseAnalyzerTestFixture
    {
        private DiagnosticDescriptor testedDiagnosticRule;

        public virtual DiagnosticDescriptor TestedDiagnosticRule
        {
            get
            {
                if (testedDiagnosticRule != null)
                    return testedDiagnosticRule;

                // TODO: This will need major refactoring if a new diagnostic group will be introduced
                var thisType = GetType();
                var ruleID = thisType.Name.Substring(0, "GA0000".Length);
                return testedDiagnosticRule = GADiagnosticDescriptorStorage.Instance.GetDiagnosticDescriptor(ruleID);
            }
        }
    }
}
