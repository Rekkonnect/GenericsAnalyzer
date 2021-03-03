﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenericsAnalyzer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GenericsAnalyzer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; has specified explicit type constraints, preventing the type &apos;{1}&apos; from being used.
        /// </summary>
        internal static string GA0001_Description {
            get {
                return ResourceManager.GetString("GA0001_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{1}&apos; cannot be used as a generic type argument for the type &apos;{0}&apos; in this position.
        /// </summary>
        internal static string GA0001_MessageFormat {
            get {
                return ResourceManager.GetString("GA0001_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prohibited type argument.
        /// </summary>
        internal static string GA0001_Title {
            get {
                return ResourceManager.GetString("GA0001_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type parameter &apos;{0}&apos; already includes a type constraint rule for the type &apos;{1}&apos;.
        /// </summary>
        internal static string GA0002_MessageFormat {
            get {
                return ResourceManager.GetString("GA0002_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot apply multiple constraint rules for the same type.
        /// </summary>
        internal static string GA0002_Title {
            get {
                return ResourceManager.GetString("GA0002_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type parameter &apos;{0}&apos; already includes a type constraint rule for the unbound version of the generic type &apos;{1}&apos;.
        /// </summary>
        internal static string GA0003_MessageFormat {
            get {
                return ResourceManager.GetString("GA0003_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot include both a bound and an unbound version of the same type.
        /// </summary>
        internal static string GA0003_Title {
            get {
                return ResourceManager.GetString("GA0003_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; is an invalid type argument.
        /// </summary>
        internal static string GA0004_MessageFormat {
            get {
                return ResourceManager.GetString("GA0004_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot assign an invalid type argument as a type constraint.
        /// </summary>
        internal static string GA0004_Title {
            get {
                return ResourceManager.GetString("GA0004_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; is prohibited from being used as a type argument for the type parameter &apos;{1}&apos;.
        /// </summary>
        internal static string GA0005_MessageFormat {
            get {
                return ResourceManager.GetString("GA0005_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type prohibited from generic type constraints in the constraint clause.
        /// </summary>
        internal static string GA0005_Title {
            get {
                return ResourceManager.GetString("GA0005_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prefer declaring the constraint of a single base bound interface type in the constraints clause.
        /// </summary>
        internal static string GA0006_MessageFormat {
            get {
                return ResourceManager.GetString("GA0006_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Declaration of single base bound interface type.
        /// </summary>
        internal static string GA0006_Title {
            get {
                return ResourceManager.GetString("GA0006_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly permitted exact types; the only restrictions are explicit prohibited exact types.
        /// </summary>
        internal static string GA0007_MessageFormat {
            get {
                return ResourceManager.GetString("GA0007_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly permitted exact types.
        /// </summary>
        internal static string GA0007_Title {
            get {
                return ResourceManager.GetString("GA0007_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant base type rule; the given base type &apos;{0}&apos; is sealed.
        /// </summary>
        internal static string GA0008_MessageFormat {
            get {
                return ResourceManager.GetString("GA0008_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant base type rule.
        /// </summary>
        internal static string GA0008_Title {
            get {
                return ResourceManager.GetString("GA0008_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; was declared more than once in the same constraint rule.
        /// </summary>
        internal static string GA0009_MessageFormat {
            get {
                return ResourceManager.GetString("GA0009_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate type rule.
        /// </summary>
        internal static string GA0009_Title {
            get {
                return ResourceManager.GetString("GA0009_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly prohibited exact types; the only permitted types are exactly specified.
        /// </summary>
        internal static string GA0010_MessageFormat {
            get {
                return ResourceManager.GetString("GA0010_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly prohibited exact types.
        /// </summary>
        internal static string GA0010_Title {
            get {
                return ResourceManager.GetString("GA0010_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly permitted exact types; no prohibitions or explicit specification for only permitting the specified permitted types via OnlyPermitSpecifiedTypesAttribute.
        /// </summary>
        internal static string GA0011_MessageFormat {
            get {
                return ResourceManager.GetString("GA0011_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant explicitly permitted exact types.
        /// </summary>
        internal static string GA0011_Title {
            get {
                return ResourceManager.GetString("GA0011_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage of the attribute requires that at least one type is permitted.
        /// </summary>
        internal static string GA0012_MessageFormat {
            get {
                return ResourceManager.GetString("GA0012_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid usage of OnlyPermitSpecifiedTypesAttribute.
        /// </summary>
        internal static string GA0012_Title {
            get {
                return ResourceManager.GetString("GA0012_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The only explicitly permitted type argument for the &apos;{0}&apos; type parameter is &apos;{1}&apos;.
        /// </summary>
        internal static string GA0013_MessageFormat {
            get {
                return ResourceManager.GetString("GA0013_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one type is explicitly permitted.
        /// </summary>
        internal static string GA0013_Title {
            get {
                return ResourceManager.GetString("GA0013_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of the attribute; the element &apos;{0}&apos; cannot inherit other types.
        /// </summary>
        internal static string GA0014_MessageFormat {
            get {
                return ResourceManager.GetString("GA0014_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of InheritBaseTypeUsageConstraintsAttribute.
        /// </summary>
        internal static string GA0014_Title {
            get {
                return ResourceManager.GetString("GA0014_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of the attribute; the type &apos;{0}&apos; does not inherit other types.
        /// </summary>
        internal static string GA0015_MessageFormat {
            get {
                return ResourceManager.GetString("GA0015_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of InheritBaseTypeUsageConstraintsAttribute.
        /// </summary>
        internal static string GA0015_Title {
            get {
                return ResourceManager.GetString("GA0015_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of the attribute; the type parameter &apos;{0}&apos; is not used in any of the base types.
        /// </summary>
        internal static string GA0016_MessageFormat {
            get {
                return ResourceManager.GetString("GA0016_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant usage of InheritBaseTypeUsageConstraintsAttribute.
        /// </summary>
        internal static string GA0016_Title {
            get {
                return ResourceManager.GetString("GA0016_Title", resourceCulture);
            }
        }
    }
}
