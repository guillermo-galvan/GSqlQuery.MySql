using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GSqlQuery.MySql.Test
{
    internal class Helper
    {
        private static string connection = null;

        internal static string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                        .AddEnvironmentVariables().AddUserSecrets(typeof(Helper).GetTypeInfo().Assembly);

                connection = builder.Build().GetConnectionString("TEST");
            }


            return connection;

        }

        private static bool IsFirtsExecute = true;
        private readonly static Mutex mut = new Mutex();

        internal static void CreateDatatable()
        {
            mut.WaitOne();

            if (IsFirtsExecute)
            {
                using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
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