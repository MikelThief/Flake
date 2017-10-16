/*
 * 
 * File:    General_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Provides some functionality to communicate with MSSQL Database. 
 */


using System;
using System.Data.SqlClient;

namespace Flake
{
    class Database_Toolbox
    {
        // Currently set to connect to TSDBHQ120 to the DEVMetastorm DB (Development Environment)
        /// <summary>
        /// Gets the version of a specified library from the TSADBHQ120 server, from Metastorm database.
        /// </summary>
        /// <param name="libraryName">Name of the library.</param>
        /// <returns>Version of the library.</returns>
        public static int GetLibraryVersion(string libraryName)
        {
            var libraryVersion = -1; // initializing variable with wrong number indicating an error when returned
            SqlConnection con = null;
            try
            {
                con = new SqlConnection("Server=TSDBHQ120; User id=flake; Password=snowflake; Database=Metastorm; Packet Size=4096");
                SqlCommand cmd = new SqlCommand(
            @"SELECT MAX([eVersion])
            FROM [DEVMetastorm].[dbo].[eProcedure] 
            WHERE [eProcedureName] = @libraryName", con);
                cmd.Parameters.AddWithValue("@libraryName", libraryName);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    libraryVersion = reader.GetInt32(0);
                }
                reader.Close();
                return libraryVersion;
            }
            catch (SqlException SQLe)
            {
                Console.WriteLine("\nBrak danych z bazy o wersji biblioteki z bazy danych!");
                Console.WriteLine(SQLe.Message);
                return -1;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
