﻿using System;

namespace GenericsAnalyzer.Test.Resources
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        public ExampleAttribute(params object[] _) { }
    }
}
