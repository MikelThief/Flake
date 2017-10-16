/*
 * 
 * File:    File_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Serves as a toolbox for operations on BPMProj files.
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;


namespace Flake
{
    class LibChange_Toolbox
    {
        /// <summary>
        /// Changes the libraries versions in the project to those set by the parameters.
        /// Ignores non-project elements.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="commonLibVer"></param>
        /// <param name="commonLibraryLibVer"></param>
        public static void PerformLibChange(string folderPath, int commonLibVer, int commonLibraryLibVer)
        {
            List<string> projectDirectoryList = new List<string>();

            General_Toolbox.PrintContinously("Sprawdzanie poprawności ścieżki: ");

            List<string> solutionDirectoryList = File_Toolbox.CollectFoldersList(folderPath, "Sol");

            // if directory is empty an exception is thrown - there is nothing to do
            if (!solutionDirectoryList.Any())
            {
                General_Toolbox.PrintState((OperationState.FAILURE));
                throw new Empty_Directory_Exception("Brak projektów w folderze! Wyjątek wystąpił w Update_Toolbox.PerformUpdate()");
            }

            General_Toolbox.PrintState((OperationState.SUCCESS));
            solutionDirectoryList.Sort();

            General_Toolbox.PrintContinouslyLine("Znalezione solucje: " + solutionDirectoryList.Count());

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nDEBUG INFO:");
                Console.ForegroundColor = ConsoleColor.White;
                for (int index = 0; index < solutionDirectoryList.Count; index++)
                {
                    Console.WriteLine(index + ". " + solutionDirectoryList[index]);
                }
            }
            // trimming from unnecessary folder elements - libs

            Console.WriteLine();
            General_Toolbox.PrintContinouslyLine("Ignorowanie projektów bibliotek i elementów pobocznych.");
            Console.WriteLine();

            solutionDirectoryList.Remove("TestSVNSol");
            solutionDirectoryList.Remove("CommonLibrarySol");
            solutionDirectoryList.Remove("CommonSol");
            solutionDirectoryList.Remove("DSAAdministracjaSol");

            General_Toolbox.PrintContinouslyLine("Biblioteka CommonLib w wersji V" + commonLibVer);
            General_Toolbox.PrintContinouslyLine("Biblioteka CommonLibraryLib w wersji V" + commonLibraryLibVer);
            Console.WriteLine();

            /* Preparing list of project folders in solution folders
             * 
             * Assuming there is only one folder inside solution folder!
             */

            for (int i = 0; i < solutionDirectoryList.Count(); i++)
            {

                foreach (var index in System.IO.Directory.GetDirectories(folderPath + @"\" + solutionDirectoryList[i]))
                {
                    var dirName = new DirectoryInfo(index).Name;
                    projectDirectoryList.Add(dirName);

                    if (System.Diagnostics.Debugger.IsAttached) // check for DEBUG configuration
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nDEBUG INFO:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(solutionDirectoryList[i] + "---->" + projectDirectoryList[i]);
                        Console.WriteLine();
                    }
                }
            }

            for (int j = 0; j < projectDirectoryList.Count(); j++)
            {

                try
                {

                        XDocument xDoc = XDocument.Load(folderPath + @"\" + solutionDirectoryList[j] + @"\" + projectDirectoryList[j] + @"\" + projectDirectoryList[j] + @".BPMProj");
                        XNamespace xNameSpace = "http://schema.metastorm.com/Metastorm.Common.Markup";
                        var reader = xDoc.CreateReader();
                        var manager = new XmlNamespaceManager(reader.NameTable);
                        const string expression = ".//x:Object[@x:Value and @x:Type='{pref_-1405111153:LibraryReference}']";

                        manager.AddNamespace("x", "http://schema.metastorm.com/Metastorm.Common.Markup");

                        var XNodeList = xDoc.XPathSelectElements(expression, manager);

                        //Console.WriteLine(XNodeList.First().Attributes());




                        //"{{http://schema.metastorm.com/Metastorm.Common.Markup}Value}"

                        foreach (var element in XNodeList)
                        {
                            bool CLLUpdated = false;
                            bool CLUpdated = false;
                            var attList = element.Attributes().ToList();
                            var oldAtt = attList.Last();


                            if ((element.LastAttribute.Value).Contains(@"CommonLib;"))
                            {
                                if (oldAtt != null)
                                {
                                    XAttribute newAtt = new XAttribute(attList.Last().Name,
                                                                        @"3eff89cc-982d-43c6-97e8-e2df5fc00e4a;Metastorm.Model.Projects.BpmLibrary, Metastorm.Model.Bpm;;dev axa;CommonLib;" + commonLibVer.ToString());
                                    attList.Add(newAtt);
                                    attList.Remove(oldAtt);
                                    element.ReplaceAttributes(attList);
                                    xDoc.Save(folderPath + @"\" + solutionDirectoryList[j] + @"\" + projectDirectoryList[j] + @"\" + projectDirectoryList[j] + @".BPMProj");
                                    CLUpdated = true;
                                }
                                else throw new Undefined_Element_Exception(@"Atrybut 'Value' dla CommonLib nie ma wartości. Plik może być uszkodzony.");
                            }
                            else if ((element.LastAttribute.Value).Contains(@"CommonLibraryLib;"))
                            {
                                if (oldAtt != null)
                                {
                                    XAttribute newAtt = new XAttribute(attList.Last().Name,
                                                                        @"3eff89cc-982d-43c6-97e8-e2df5fc00e4a;Metastorm.Model.Projects.BpmLibrary, Metastorm.Model.Bpm;;dev axa;CommonLibraryLib;" + commonLibraryLibVer.ToString());
                                    attList.Add(newAtt);
                                    attList.Remove(oldAtt);
                                    element.ReplaceAttributes(attList);
                                    xDoc.Save(folderPath + @"\" + solutionDirectoryList[j] + @"\" + projectDirectoryList[j] + @"\" + projectDirectoryList[j] + @".BPMProj");
                                    CLLUpdated = true;
                                }
                                else throw new Undefined_Element_Exception(@"Atrybut 'Value' dla CommonLibraryLib nie ma wartości. Plik może być uszkodzony.");
                            }

                            General_Toolbox.PrintContinously("Zaktualizowano ");

                            if (CLUpdated && !CLLUpdated)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("CommonLib");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (!CLUpdated && CLLUpdated)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("CommonLibraryLib");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (CLLUpdated && CLLUpdated)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("CommonLib");
                                Console.ForegroundColor = ConsoleColor.White;
                                General_Toolbox.PrintContinously("oraz ");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("CommonLibraryLib");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else General_Toolbox.PrintContinously("0 bibliotek");

                            General_Toolbox.PrintContinouslyLine(" w projekcie " + projectDirectoryList[j] + @".BPMProj");

                        }
                }
                catch (XmlException ex)
                { Console.WriteLine(ex.Message); }
                catch (Undefined_Element_Exception e)
                { Console.WriteLine(e.Message); };

             }

            }
    }
}
