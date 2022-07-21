using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIStudio
{
    public static class StudioManager
    {
        public static async Task<string> onCompile(string name)
        {
            string strCommand = "cmd.exe";
            string strCommandParameters = $"/c dotnet publish -r win-x64 -c debug {name}";
            string strWorkingDirectory = "..\\";

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
            pProcess.StartInfo.WorkingDirectory = strWorkingDirectory;
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
