/*
 * 
 * File:    File_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Serves as a toolbox to control external DEPLOY.EXE tool. 
 */

using System;
using System.Diagnostics;

namespace Flake
{
    class Deploy_Toolbox
    {

        /// <summary>
        /// Invokes Deploy.exe tool with provided commandline parameters. Currently only upload MBMP projects, excluding libraries.
        /// </summary>
        /// <param name="opentextPath">Path to folder containing Metastorm projects. 
        /// All projects in the folder are recursively searched and then uploaded.</param>
        /// <param name="deployToolPath">Path to "deploy.exe: tool.</param>
        /// <param name="deploymentServiceConfigPath">Path to deployment service configuration file in XML.</param>
        /// <param name="performIntegratedUpdate">Flag indicating whether an update of libraries in projects should be performed. 
        /// This update uses internal deploy.exe mechanism</param>
        public static void PerformDeploy(string opentextPath, string deployToolPath, string deploymentServiceConfigPath, bool performIntegratedUpdate)
        {
            if (!File_Toolbox.IsFileLocked(deployToolPath) && !File_Toolbox.IsFileLocked(deploymentServiceConfigPath))
            {
                Console.WriteLine();
                General_Toolbox.PrintContinouslyLine("Uruchamianie narzędzia DEPLOY.EXE z podanymi argumentami");
                General_Toolbox.PrintContinouslyLine("Narzędzie wyśle tylko projekty. Wysyłanie bibliotek jest obecnie wyłączone.");
                General_Toolbox.PrintContinouslyLine("Wysyłanie na środowisko DEV...");

                ProcessStartInfo processInfo = new ProcessStartInfo();

                string argumentsLine = "/dir;" + opentextPath + ";bpmproj" + " " + "/deploymentservice;" + deploymentServiceConfigPath + ";" + "flake;snowflake";

                if (performIntegratedUpdate)
                {
                    argumentsLine += " /forcelibmatch";
                    General_Toolbox.PrintContinouslyLine("Włączone użycie wewnętrzego mechanizmu aktualizacji bibliotek.");
                }
                else General_Toolbox.PrintContinouslyLine("Wyłączenie wewnętrzego mechanizmu aktualizacji bibliotek.");

                processInfo.Arguments = argumentsLine;
                processInfo.CreateNoWindow = false;
                processInfo.ErrorDialog = true;
                processInfo.UseShellExecute = true;
                processInfo.FileName = deployToolPath;
                processInfo.WindowStyle = ProcessWindowStyle.Normal;


                using (Process deployexe = Process.Start(processInfo))
                {
                    deployexe.WaitForExit();
                }




            }
            else General_Toolbox.PrintContinouslyLine("Program Flake nie był w stanie wywołać narzędzia 'DEPLOY.EXE' ze względu na błędną konfigurację");
        }


    }
}
