using GSqlQuery.MySql.Benchmark.Data;
using GSqlQuery.Runner;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

namespace GSqlQuery.MySql.Benchmark
{
    internal static class CreateTable
    {
        internal const string ConnectionString = "server=127.0.0.1;uid=root;pwd=saadmin;";

        internal static MySqlConnectionOptions GetConnectionOptions()
        {
            return new MySqlConnectionOptions(ConnectionString);
        }

        internal static void Create()
        {
            var tables = Tables.Select(GetConnectionOptions()).Build().Execute();

            if (tables == null || !tables.Any())
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var createCommand = connection.CreateCommand())
                    {
                        createCommand.CommandText =
                       @"
                            -- -----------------------------------------------------
                            -- Schema GSQLQuery
                            -- -----------------------------------------------------
                            DROP SCHEMA IF EXISTS `GSQLQuery` ;

                            -- -----------------------------------------------------
                            -- Schema GSQLQuery
                            -- -----------------------------------------------------
                            CREATE SCHEMA IF NOT EXISTS `GSQLQuery` DEFAULT CHARACTER SET utf8 ;
                            USE `GSQLQuery` ;

                            -- -----------------------------------------------------
                            -- Table `GSQLQuery`.`Test1`
                            -- -----------------------------------------------------
                            CREATE TABLE IF NOT EXISTS `GSQLQuery`.`Test1` (
                              `idTest1` BIGINT NOT NULL,
                              `Money` DECIMAL(23,9) NULL,
                              `Nombre` VARCHAR(120) NULL,
                              `GUID` VARCHAR(100) NULL,
                              `URL` VARCHAR(200) NULL)
                            ENGINE = InnoDB;


                            -- -----------------------------------------------------
                            -- Table `GSQLQuery`.`Test2`
                            -- -----------------------------------------------------
                            CREATE TABLE IF NOT EXISTS `GSQLQuery`.`Test2` (
                              `Id` BIGINT NOT NULL AUTO_INCREMENT,
                              `Money` DECIMAL(23,9) NULL,
                              `IsBool` BIT NULL,
                              `Time` DATETIME NULL,
                              PRIMARY KEY (`Id`))
                            ENGINE = InnoDB;
                        ";
                        createCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void CreateData()
        {
            var connectionOptions = GetConnectionOptions();
            using (var connection = connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var batch = Execute.BatchExecuteFactory(connectionOptions);
                    Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
                    for (int i = 0; i < 1000; i++)
                    {
                        test.IsBool = (i % 2) == 0;
                        batch.Add(sb => test.Insert(sb).Build());
                    }

                    int result = batch.Execute(transaction.Connection);

                    transaction.Commit();
                }

                connection.Close();
            }
        }
    }
}