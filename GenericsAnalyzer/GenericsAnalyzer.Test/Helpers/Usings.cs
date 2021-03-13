namespace GenericsAnalyzer.Test.Helpers
{
    public static class UsingsHelpers
    {
        public const string DefaultNecessaryUsings =
@"
using GenericsAnalyzer.Core;
using GenericsAnalyzer.Test.Resources;
using System;
using System.Collections.Generic;
";

        public static string WithUsings(string original) => WithUsings(original, DefaultNecessaryUsings);
        public static string WithUsings(string original, string usings) => $"{usings}\n{original}";
    }
}
