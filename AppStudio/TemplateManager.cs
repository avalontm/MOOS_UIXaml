using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIStudio
{
    public static class TemplateManager
    {
        public static string Generate()
        {
            string result = "<Project Sdk=\"Microsoft.NET.Sdk\">\n";
            result += "<PropertyGroup>\n";

            result += "<OutputType>Exe</OutputType>\n";
            result += "<TargetFramework>net6.0</TargetFramework>\n";
            result += "<AllowUnsafeBlocks>true</AllowUnsafeBlocks>\n";
            result += "<BaseOutputPath>..\\bin</BaseOutputPath>\n";
            result += "<Optimize>true</Optimize>\n";

            result += "<GenerateAssemblyInfo>false</GenerateAssemblyInfo>\n";
            result += "<GenerateTargetFrameworkAttribute>false</GenerateTargetFrameworkAttribute>\n";
            result += "<ErrorOnDuplicatePublishOutputFiles>false</ErrorOnDuplicatePublishOutputFiles>\n";

            result += "<IlcSystemModule>ConsoleApp1</IlcSystemModule>\n";
            result += "<EntryPointSymbol>Main</EntryPointSymbol>\n";
            result += "<LinkerSubsystem>NATIVE</LinkerSubsystem>\n";
            result += "<IlcOptimizationPreference>Speed</IlcOptimizationPreference>\n";

            result += "</PropertyGroup>\n";

            result += "<ItemGroup>\n";
            result += "<LinkerArg Include=\"/filealign:0x1000\"/>\n";
            result += "</ItemGroup>\n";

            result += "<ItemGroup>\n";
            result += "<PackageReference Include=\"Microsoft.DotNet.ILCompiler\" Version=\"7.0.0-alpha.1.22074.1\" />\n";
            result += "</ItemGroup>\n";

            result += "<Target Name=\"CustomizeReferences\" BeforeTargets=\"BeforeCompile\" AfterTargets=\"FindReferenceAssembliesForReferences\">\n";
            result += "<ItemGroup/>\n";
            result += "<ReferencePathWithRefAssemblies Remove=\"@(ReferencePathWithRefAssemblies)\" />\n";
            result += "<ReferencePath Remove=\"@(ReferencePath)\" />\n";
            result += "</ItemGroup>\n";
            result += "<Import Project=\"..\\Corlib\\Corlib.projitems\" Label=\"Shared\" />\n";
            result += "<Project/>\n";

            return result;
        }
    }
}
