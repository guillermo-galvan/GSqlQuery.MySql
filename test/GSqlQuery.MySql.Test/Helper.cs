using MySql.Data.MySqlClient;
using System.Threading;

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

                        createCommand.CommandText =
                        @"
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL) 
                                            VALUES (1,'120.32','Test desde la app','sdsadsa','test');
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL) 
                                            VALUES (2,'120.32','Test desde la app','sdsadsa','test');
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL) 
                                            VALUES (3,'120.32','Test desde la app','sdsadsa','test');
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL) 
                                            VALUES (4,'120.32','Test desde la app','sdsadsa','test');
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL) 
                                            VALUES (5,'120.32','Test desde la app','sdsadsa','test');
                           INSERT INTO test1 (idTest1,Money,Nombre,GUID,URL)
                                            VALUES (6,'120.32','Test desde la app','sdsadsa','test');
                        ";
                        createCommand.ExecuteNonQuery();

                        createCommand.CommandText =
                       @"
                           INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                           INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                           INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                           INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                            INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                           INSERT INTO test2 (Money,IsBool,Time ) 
                                        VALUES ('1235.23',1,null);
                        ";
                        createCommand.ExecuteNonQuery();

                        IsFirtsExecute = false;
                    }
                }
            }

            mut.ReleaseMutex();
        }
    }
}