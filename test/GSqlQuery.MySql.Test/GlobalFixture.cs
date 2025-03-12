using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Threading.Tasks;
using Testcontainers.MySql;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class GlobalFixture : IAsyncLifetime
    {
        public const string CONNECTIONSTRING =  "server=127.0.0.1;uid=root;pwd=saadmin;port=9000;";

        private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:latest")
            .WithPortBinding(9000, 3306)
            .WithName("GSqlQuery.Mysql-db-test")
            .WithPassword("saadmin")
            .WithDatabase("sakila")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync()
        {
            await _mySqlContainer.StartAsync();
            CreateDatatable();
        }

        public async Task DisposeAsync()
        {
            await _mySqlContainer.StopAsync();
        }

        internal static void CreateDatatable()
        {

            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                connection.Open();

                var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "sakila-schema.sql");
                var script = new MySqlScript(connection, File.ReadAllText(path));
                script.Execute();

                path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "sakila-data.sql");
                script = new MySqlScript(connection, File.ReadAllText(path));
                script.Execute();
            }
        }
    }
}
