/*
 * 
 * File:    Trac_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Provides functionality used in TRAC mode.  
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Flake
{
    class Trac_Toolbox
    {
        /// <summary>
        ///  Creates a txt file in the project folder using data from source variable.
        /// </summary>
        /// <param name="folderPath">Source string to be put into the note.</param>

        public static void MakeNote(string folderPath)
        {
            General_Toolbox.PrintContinously("Sprawdzanie poprawności ścieżki: ");

            var directoryList = File_Toolbox.CollectFoldersList(folderPath, "Sol");

            // if directory is empty an exception is thrown - there is nothing to do
            if (!directoryList.Any())
            {
                General_Toolbox.PrintState(OperationState.FAILURE);
                throw new Empty_Directory_Exception("Brak projektów w folderze! Wyjątek wystąpił w Trac_Toolbox.MakeNote()");
            }

            General_Toolbox.PrintState(OperationState.SUCCESS);
            
            directoryList.Sort();

            General_Toolbox.PrintContinouslyLine("Znalezione solucje:");

            for(int index = 0; index < directoryList.Count; index++)
              { Console.WriteLine(index + ". " + directoryList[index]); }

            // trimming from unnecessary folder elements - libs

            Console.WriteLine();
            General_Toolbox.PrintContinouslyLine("Ignorowanie projektów bibliotek.");
            Console.WriteLine();

            directoryList.Remove("TestSVNSol");
            directoryList.Remove(".svn");
            directoryList.Remove("DSAAdministracjaSol");
            directoryList.Remove("CommonLibrarySol");
            directoryList.Remove("CommonSol");          

            // generating a note and inserting it into every folder to cause track to redeploy whole solution
            General_Toolbox.PrintContinouslyLine("Notatki dla projektów: ");
            for (int index = 0; index < directoryList.Count; index++)
            {
                if (!File.Exists(folderPath + @"\" + directoryList[index] + @"\TracNote.txt"))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(folderPath + @"\" + directoryList[index] + @"\TracNote.txt"))
                        { 
                            sw.WriteLine("Ostatnia modyfikacja dla narzędzia Trac: " + DateTime.Now.ToString());
                            sw.WriteLine("Utworzył: " + Environment.UserName);
                            sw.Close();
                            Console.WriteLine(index + ". " + directoryList[index]);
                        }
                }
                else if (!File_Toolbox.IsFileLocked(folderPath + @"\" + directoryList[index] + @"\TracNote.txt"))
                {
                    using (FileStream f = new FileStream(folderPath + @"\" + directoryList[index] + @"\TracNote.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(f))
                        sw.WriteLine("Ostatnia modyfikacja dla narzedzia TRAC: " + DateTime.Now.ToString());

                }
                else throw new IOException("Nie można uzyskać dostępu do plików.");
            }
            General_Toolbox.PrintContinouslyLine("zostały utworzone.");



        }
    }
}
