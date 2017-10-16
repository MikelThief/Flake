/*
 * 
 * File:    File_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Serves as a toolbox to operate on files. 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Flake
{
    class File_Toolbox
    {     
        /// <summary>
        /// Checks if file is locked by another process.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>State: Locked (True), Unlocked (False)</returns>
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);
                Console.WriteLine(e.Message);
                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }

        /// <summary>
        /// Gets a alphabetically sorted list of folders in a specified directory.
        /// Directory's name must end with "ending" ending.
        /// </summary>
        /// <param name="path">Path to look for directories</param>
        /// <param name="ending">First ending string</param>
        /// <returns>Alphabetically sorted list.</returns>
        public static List<string> CollectFoldersList(string path, string ending)
        {
            return System.IO.Directory.GetDirectories(path).Select(index => new DirectoryInfo(index).Name).Where(dirName => dirName.EndsWith("Sol")).ToList();

             // Replaced with LINQ expression above
            /*
             foreach (var index in System.IO.Directory.GetDirectories(folderPath))
            {
                var dirName = new DirectoryInfo(index).Name;
                if(dirName.EndsWith("Sol"))
                    directoryList.Add(dirName);
                //Console.WriteLine(dirName);
            }
            */

        }
        /// <summary>
        /// Gets a alphabetically sorted list of folders in a specified directory.
        /// Directory's name must either end with "ending1" or "ending2" ending.
        /// </summary>
        /// <param name="path">Path to look for directories</param>
        /// <param name="ending1">First ending string</param>
        /// <param name="ending2">Second ending string</param>
        /// <returns>Alphabetically sorted list.</returns>
        public static List<string> CollectFoldersList(string path, string ending1, string ending2)
        {
            List<string> list = System.IO.Directory.GetDirectories(path).Select(index => new DirectoryInfo(index).Name).Where(dirName => (dirName.EndsWith(ending1) || dirName.EndsWith(ending2))).ToList();
            list.Sort();

            return list;

            // Replaced with LINQ expression above
            /*
             foreach (var index in System.IO.Directory.GetDirectories(folderPath))
            {
                var dirName = new DirectoryInfo(index).Name;
                if(dirName.EndsWith("Sol"))
                    directoryList.Add(dirName);
                //Console.WriteLine(dirName);
            }
            */

        }
    }
}
