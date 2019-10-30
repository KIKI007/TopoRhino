using System.Reflection;
using System.Runtime.InteropServices;
using Rhino.PlugIns;

// Plug-in Description Attributes - all of these are optional
// These will show in Rhino's option dialog, in the tab Plug-ins
[assembly: PlugInDescription(DescriptionType.Address, "EPFL")]
[assembly: PlugInDescription(DescriptionType.Country, "Switzerland")]
[assembly: PlugInDescription(DescriptionType.Email, "qiqiustc@gmail.com")]

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle("TopoRhino")]
[assembly: AssemblyDescription("Topological Interlocking")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("EPFL")]
[assembly: AssemblyProduct("TopoRhino")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Rhino requires a Guid assigned to the assembly. Xamarin Studio can't insert a Guid in file templates automatically.
[assembly: Guid("0b79bf6d-9d2c-4c19-84ff-d015c84b0a6b")]
