using GSqlQuery.MySql.BulkCopy;
using GSqlQuery.MySql.Test.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test.BulkCopy
{
    public class BulkCopyFactoryTest
    {
        private readonly string CONNECTIONSTRING = Helper.ConnectionString + "AllowLoadLocalInfile=true;AllowUserVariables=True;";


        public BulkCopyFactoryTest()
        {
            Helper.CreateDatatable();
        }

        private IEnumerable<Test1> GetTest1s()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "Test1_10000.csv");
            Queue<Test1> list = new Queue<Test1>();

            foreach (var item in File.ReadAllLines(path))
            {
                string[] columns = item.Split(',');
                list.Enqueue(new Test1() { Id = Convert.ToInt64(columns[0]), Money = Convert.ToDecimal(columns[1]), Nombre = columns[2], GUID = columns[3], URL = columns[4] });
            }

            return list;
        }

        private IEnumerable<Test2> GetTest2s()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "Test2_10000.csv");
            Queue<Test2> list = new Queue<Test2>();

            foreach (var item in File.ReadAllLines(path))
            {
                string[] columns = item.Split(',');
                list.Enqueue(new Test2() { Money = Convert.ToDecimal(columns[0]), IsBool = Convert.ToBoolean(columns[1] == "1"), Time = Convert.ToDateTime(columns[2]) });
            }

            return list;
        }

        [Fact]
        public void Throw_exeception()
        {
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(null));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(string.Empty));
            Assert.Throws<InvalidOperationException>(() => BulkCopyFactory.Create(Helper.ConnectionString));
            Assert.Throws<InvalidOperationException>(() => BulkCopyFactory.Create(Helper.ConnectionString + "AllowLoadLocalInfile=true;"));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(Helper.ConnectionString + "AllowLoadLocalInfile=true;AllowUserVariables=True;", null));
        }

        [Fact]
        public void Execute()
        {
            var data = GetTest2s();
            var data1 = GetTest1s();

            var beforeTotal = Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();
            var beforeTotal1 = Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();

            var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).Copy(data1).Execute();

            var total = Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();
            var total1 = Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();

            Assert.True(bulkcopy == 20000);
            Assert.True(total > beforeTotal);
            Assert.True(total1 > beforeTotal1);
        }

        [Fact]
        public void Execute_with_connection()
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                var data = GetTest2s();
                var data1 = GetTest1s();

                var beforeTotal = Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();
                var beforeTotal1 = Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();

                var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).Copy(data1).Execute(connection);

                var total = Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();
                var total1 = Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString)).Build().Execute().Count();

                Assert.True(bulkcopy == 20000);
                Assert.True(total > beforeTotal);
                Assert.True(total1 > beforeTotal1);
                connection.Close();
            }
        }

        [Fact]
        public void Execute_with_connection_Throw_exeception()
        {
            var data = GetTest2s();
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).Execute(null));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var data = GetTest2s();
            var data1 = GetTest1s();

            var beforeTotal = await Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();
            var beforeTotal1 = await Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();

            var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).Copy(data1).ExecuteAsync();

            var total = await Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();
            var total1 = await Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();

            Assert.True(bulkcopy == 20000);
            Assert.True(total > beforeTotal);
            Assert.True(total1 > beforeTotal1);
        }

        [Fact]
        public async Task ExecuteAsync_with_connection()
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                await connection.OpenAsync();
                var data = GetTest2s();
                var data1 = GetTest1s();

                var beforeTotal = await Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();
                var beforeTotal1 = await Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();

                var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).Copy(data1).ExecuteAsync(connection);

                var total = await Test2.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();
                var total1 = await Test1.Select(new MySqlConnectionOptions(Helper.ConnectionString), x => x.Id).Count().Build().ExecuteAsync();

                Assert.True(bulkcopy == 20000);
                Assert.True(total > beforeTotal);
                Assert.True(total1 > beforeTotal1);
                connection.Close();
            }
        }

        [Fact]
        public async Task ExecuteAsync_with_connection_Throw_exeception()
        {
            var data = GetTest2s();
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(data).ExecuteAsync(null));
        }
    }
}