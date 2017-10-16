/*
 * 
 * File:    General_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Provides some general functionality tools to communicate with the other 
 * elements of the program.
 */

using System;
using System.Threading;
using System.IO;
using CommandLine;


namespace Flake
{
    internal enum OperationState
    {
        SUCCESS,
        FAILURE
    }

    class General_Toolbox
    {
        /// <summary>
        /// Prints welcome message to the user.
        /// </summary>
        public static void PrintWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Flake - OpenText Metastorm Assistant");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
        }
        /// <summary>
        /// Processes commandline arguments according to the rules set in CMD_Parser_Options.
        /// </summary>
        /// <param name="srcArgs">Arguments taken from commandline.</param>
        public static void ProcessArguments(string[] srcArgs)
        {
            var options = new CMD_Parser_Options();
            if (Parser.Default.ParseArguments(srcArgs, options))
            {
                PrintContinously("Tryb pracy: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(options.SpecifiedWorkingMode);
                Console.ForegroundColor = ConsoleColor.White;

                switch (options.SpecifiedWorkingMode)
                {
                    case WorkingMode.TRAC:

                        PrintContinously("Lokalizacja folderu OpenText: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;


                        if (options.OpenTextFolderPath != null)
                        {
                            Console.WriteLine(options.OpenTextFolderPath.ToUpper());
                            Console.ForegroundColor = ConsoleColor.White;
                            try { Trac_Toolbox.MakeNote(options.OpenTextFolderPath); }
                            catch (Empty_Directory_Exception exc)
                            {
                                Console.WriteLine(exc.Message);
                            }
                            catch (IOException exc)
                            {
                                Console.WriteLine(exc.Message); 
                            }
                        }
                        else throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu OpenText!");
                        break;

                    case WorkingMode.DEPLOY:

                        PrintContinously("Lokalizacja folderu OpenText: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(options.OpenTextFolderPath.ToUpper());
                        Console.ForegroundColor = ConsoleColor.White;

                        PrintContinously("Lokalizacja folderu z narzędziem DEPLOY.EXE: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(options.DeploymentToolPath.ToUpper());
                        Console.ForegroundColor = ConsoleColor.White;

                        PrintContinouslyLine("Lokalizacja folderu z plikiem konfiguracyjnym Deploy Service Config: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(options.DeploymentConfigPath.ToUpper());
                        Console.ForegroundColor = ConsoleColor.White;

                        if (options.OpenTextFolderPath == null)
                            throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu OpenText!");

                        if (options.DeploymentToolPath == null)
                            throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu z narzędziem DEPLOY.EXE!");

                        if (options.DeploymentConfigPath == null)
                            throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu z plikiem konfiguracyjnym Deploy Service Config!");

                        bool forceLibMatch;

                        General_Toolbox.PrintContinouslyLine("Czy chcesz użyć wewnętrznego mechanizmu Metastorma do aktualizacji bilbiotek przed Deployem? TAK/NIE");
                        Console.Write(">>\t");
                        if (Console.ReadLine() == "TAK")
                        {
                            forceLibMatch = true;
                            try
                            { LibChange_Toolbox.PerformLibChange(options.OpenTextFolderPath, 1, 1); }
                            catch (Undefined_Element_Exception e)
                            { Console.WriteLine(e.Message); };
                        }
                        else forceLibMatch = false;

                        Deploy_Toolbox.PerformDeploy(options.OpenTextFolderPath, options.DeploymentToolPath, options.DeploymentConfigPath, forceLibMatch);

                        break;


                    case WorkingMode.UPDATE:

                        PrintContinously("Lokalizacja folderu OpenText: ");

                        if (options.OpenTextFolderPath != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(options.OpenTextFolderPath.ToUpper());
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu OpenText!");

                        try { LibChange_Toolbox.PerformLibChange(options.OpenTextFolderPath, 
                                                           Database_Toolbox.GetLibraryVersion("CommonLib"), 
                                                           Database_Toolbox.GetLibraryVersion("CommonlibraryLib")); }
                        catch (Undefined_Element_Exception e)
                        { Console.WriteLine(e.Message); };
                        break;

                    case WorkingMode.DOWNGRADE:

                        PrintContinously("Lokalizacja folderu OpenText: ");

                        if (options.OpenTextFolderPath != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(options.OpenTextFolderPath.ToUpper());
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else throw new Undefined_Element_Exception("Brak podanej lokalizacji folderu OpenText!");

                        try
                        { LibChange_Toolbox.PerformLibChange(options.OpenTextFolderPath, 1, 1); }
                        catch (Undefined_Element_Exception e)
                        { Console.WriteLine(e.Message); }
                        ;
                        break;
                }



            }
           // else Console.WriteLine(options.GetUsage()); // doubles GetUsage() call

            PrintContinouslyLine("Program zakończył swoje działanie.");
        }



        /// <summary>
        /// Prints text fluently like in movies.
        /// </summary>
        /// <param name="src">Source string to be written.</param>
        public static void PrintContinously(string src)
        {
            for (int position = 0; position < src.Length; position++)
            {
                Console.Write(src[position]);
                Thread.Sleep(20);
            }
            Thread.Sleep(120);
        }
        /// <summary>
        /// Prints text fluently like in movies.
        /// </summary>
        /// <param name="src">Source string to be written.</param>
        public static void PrintContinouslyLine(string src)
        {
            PrintContinously(src);
            Console.WriteLine();
        }

        public static void PrintState(OperationState opState)
        {
            switch (opState)
            {
                case OperationState.SUCCESS:
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("POWODZENIE");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                case OperationState.FAILURE:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("NIEPOWODZENIE");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                default:
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("NIEROZPOZNANO");
                    Console.WriteLine();
                    Environment.Exit(1);
                        break;

                }
            }



        }
    }
}
