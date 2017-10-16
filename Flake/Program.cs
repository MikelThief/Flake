/*
 * 
 * File:    General_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Main project's file.
 * 
 */

using System;

namespace Flake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Flake - OpenText Metastorm Assistant";


            General_Toolbox.PrintWelcomeMessage();

            try
            {
                General_Toolbox.ProcessArguments(args);
            }
            catch (Undefined_Element_Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }
    }
}
