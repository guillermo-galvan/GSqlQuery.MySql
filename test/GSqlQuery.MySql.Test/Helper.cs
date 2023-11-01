using MySql.Data.MySqlClient;
using System.IO;
using System;
using System.Threading;
using System.Linq;
using MySql.Data.Types;

namespace GSqlQuery.MySql.Test
{
    internal class Helper
    {
        internal const string ConnectionString = "server=127.0.0.1;uid=root;pwd=saadmin;";

        private static bool IsFirtsExecute = true;
        private readonly static Mutex mut = new Mutex();

        internal static void CreateDatatable()
        {
            mut.WaitOne();

            if (IsFirtsExecute)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    
                    var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "sakila-schema.sql");
                    var script = new MySqlScript(connection, File.ReadAllText(path));
                    script.Execute();

                    path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "sakila-data.sql");
                    script = new MySqlScript(connection, File.ReadAllText(path));
                    script.Execute();

                    IsFirtsExecute = false;
                }
            }

            mut.ReleaseMutex();
        }
    }
}