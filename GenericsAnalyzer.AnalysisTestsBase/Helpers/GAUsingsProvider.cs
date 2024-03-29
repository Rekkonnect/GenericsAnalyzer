﻿using RoseLynn.Testing;

namespace GenericsAnalyzer.AnalysisTestsBase.Helpers
{
    public sealed class GAUsingsProvider : UsingsProviderBase
    {
        public static readonly GAUsingsProvider Instance = new();

        public const string DefaultUsings =
@"
using GenericsAnalyzer.Core;
using GenericsAnalyzer.AnalysisTestsBase.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
";

        public override string DefaultNecessaryUsings => DefaultUsings;

        private GAUsingsProvider() { }
    }
}
