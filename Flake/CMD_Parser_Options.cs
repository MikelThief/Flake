/*
 *
 * File:    General_Toolbox.cs
 * Author:  Michał Bator
 *
 * Contains rules for commandline's arguments parsing.
 *
 */


using System.Text;
using CommandLine;

namespace Flake
{
    internal enum WorkingMode
    {
        TRAC,
        DOWNGRADE,
        UPDATE,
        DEPLOY
    }


    internal class CMD_Parser_Options
    {
        [Option('m', "mode", Required = true, HelpText = "Working mode.")]
        public WorkingMode SpecifiedWorkingMode { get; set; }

        [Option('o', "opentext_folder_path", Required = false, DefaultValue = null,
            HelpText = "The path containing solutions.")]
        public string OpenTextFolderPath { get; set; }

        [Option('t', "deployment_tool_path", Required = false, DefaultValue = "UNDEFINED",
            HelpText = "The path containing Metastorm 'Deploy.exe' tool.")]
        public string DeploymentToolPath { get; set; }

        [Option('c', "deployment_config_path", Required = false, DefaultValue = "UNDEFINED",
            HelpText = "The path containing Deployment Service Config XML file for 'Deploy.exe'.")]
        public string DeploymentConfigPath { get; set; }

        [HelpOption(HelpText = "Display this help screen")]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var helpMsg = new StringBuilder();
            helpMsg.AppendLine("Flake V1 - Instrukcja użytkowania");
            helpMsg.AppendLine("---------------------------------\n\n");

            helpMsg.AppendLine("Opis metod wywołania programu:");
            helpMsg.AppendLine("Parametr {-m} {--mode} przekazuje do programu określenie trybu pracy.");
            helpMsg.AppendLine("Dostępne tryby to: TRAC, DEPLOY, UPDATE, DOWNGRADE");
            helpMsg.AppendLine("\n");
            helpMsg.AppendLine(
                "Parametr {-o} {--opentext_folder_path} określa lokalizację folderu, gdzie znajdują się pliki solucji programu OpenText Metastorm (MBPM)");
            helpMsg.AppendLine("\n");
            helpMsg.AppendLine(
                "Parametr {-t} {--deployment_tool_path} określa lokalizację narzędzia Deploy.exe, pochodzącego z pakietu MBPM");
            helpMsg.AppendLine("\n");
            helpMsg.AppendLine(
                "Parametr {-c} {--deployment_config_path} określa lokalizację pliku konfiguracyjnego w formacie XML");
            helpMsg.AppendLine(
                "Domyślnie, plik powinien zawierać konfigurację dla środowiska DEV AXA, ponieważ tak skonfigurowane jest sprawdzanie wersji bilbiotek");


            helpMsg.AppendLine("\n");
            helpMsg.AppendLine("Opis trybów pracy:");
            helpMsg.AppendLine(
                "TRAC - umieszcza w katalogach projektów plik tekstowy z notatką, co zmusza TRAC do przebudowania tego projektu");
            helpMsg.AppendLine(
                "UPDATE - aktualizuje wersje bibliotek w plikach projektowych solucji. Aktualna wersja jest odczytywana z bazy DEVMetastorm (DEV AXA)");
            helpMsg.AppendLine(
                "DOWNGRADE - ustawia wersje bibliotek w plikach projektowych solucji na 1, co spowoduje ich automatyczną aktualizację w trybie DEPLOY.");
            helpMsg.AppendLine(
                "DEPLOY - uruchamia tryb DOWNGRADE, a następnie narzędzie Deploy.exe i wykonuje wdrożenie solucji na serwer deweloperski z automatyczną aktualizacją bibliotek");
            helpMsg.AppendLine("\n");
            helpMsg.AppendLine("\n");
            helpMsg.AppendLine("----------------------------------");
            return helpMsg.ToString();
        }
    }
}