# GenericsAnalyzer
A Roslyn analyzer for C# for generic types and functions

## References
- [Usage](docs/usage.md)
- [Diagnostics](docs/rules/)

## Downloads
### NuGet Packages
- [GenericsAnalyzer](https://www.nuget.org/packages/GenericsAnalyzer) - includes the analyzer and the core components
- [GenericsAnalyzer.Core](https://www.nuget.org/packages/GenericsAnalyzer.Core) - ONLY includes the core components

### Extensions
It is not recommended to use the VSIX; it does not prohibit compilation from error emission. Each project must include a reference to the above packages in order to properly function as a language extension.
- [VSIX Plugin](https://marketplace.visualstudio.com/items?itemName=Rekkon.GenericAnalyzer)

## Consumption
### Recommended way
Include a reference to the GenericsAnalyzer NuGet package in the desired projects individually.

### Other
Download and install the VSIX. Each project should then include the GenericsAnalyzer.Core NuGet package in order to use the analyzer's attributes.
