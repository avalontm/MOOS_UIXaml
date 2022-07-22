using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIStudio
{
    public static class StudioManager
    {
        public static async Task<string> onCompile(string content, string name)
        {
            string strWorkingDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "build");

            if (!Directory.Exists(strWorkingDirectory))
            {
                Directory.CreateDirectory(strWorkingDirectory);
            }

            string format = "using System.Runtime;\n";
            format += "using System.Runtime.InteropServices;\n\n";
            format += "static unsafe class Program\n";
            format += "{\n\n";
            format += "    [RuntimeExport(\"Main\")]\n";
            format += "    public static void Main()\n";
            format += "    {\n";
            format += "\n";
            format += "    }\n";
            format += "\n";
            format += content;
            format += "\n";
            format += "}\n";

            File.WriteAllText(strWorkingDirectory + "/mossapp.csproj", TemplateManager.Generate());
            File.WriteAllText(strWorkingDirectory + "/Program.cs", format);

            string strCommand = "cmd.exe";
            string strCommandParameters = $"/c dotnet publish -r win-x64 -c debug \"{strWorkingDirectory}/{name}.csproj\"";

            //Create process
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            //strCommand is path and file name of command to run
            pProcess.StartInfo.FileName = strCommand;
            //strCommandParameters are parameters to pass to program
            pProcess.StartInfo.Arguments = strCommandParameters;
            pProcess.StartInfo.UseShellExecute = false;
            //Set output of program to be written to process output stream
            pProcess.StartInfo.RedirectStandardOutput = true;
            //Optional
            //pProcess.StartInfo.WorkingDirectory = strWorkingDirectory;
            //Start the process
            pProcess.Start();
            //Get program output
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            //Wait for process to finish
            pProcess.WaitForExit();

            return strOutput;
        }
    }
}
