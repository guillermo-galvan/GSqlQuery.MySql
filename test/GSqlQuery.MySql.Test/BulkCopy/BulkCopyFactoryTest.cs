using GSqlQuery.MySql.BulkCopy;
using GSqlQuery.MySql.Test.Data;
using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.Runner;
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

        private readonly IEnumerable<Actor> _actors;
        private readonly IEnumerable<Customer> _customers;
        private readonly MySqlConnectionOptions _connection;

        public BulkCopyFactoryTest()
        {
            Helper.CreateDatatable();
            _connection = new MySqlConnectionOptions(Helper.ConnectionString);
            _actors = Actor.Select(_connection).Build().Execute();
            _customers = Customer.Select(_connection).Build().Execute();
        }

        [Fact]
        public void Throw_exeception()
        {
            BulkCopyConfiguration bulkCopyConfiguration = null;
            string connectionString = null;

            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(bulkCopyConfiguration));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(connectionString));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(string.Empty));
            Assert.Throws<InvalidOperationException>(() => BulkCopyFactory.Create(Helper.ConnectionString));
            Assert.Throws<InvalidOperationException>(() => BulkCopyFactory.Create(Helper.ConnectionString + "AllowLoadLocalInfile=true;"));
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(new BulkCopyConfiguration(Helper.ConnectionString + "AllowLoadLocalInfile=true;AllowUserVariables=True;", null)));
        }

        [Fact]
        public void Execute()
        {
            var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId}).Count().Build().Execute();
            var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute();

            var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
            var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public void Execute_with_connection()
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                var beforeActorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var beforeCustomersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();

                var bulkcopy = BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).Execute(connection);

                var actorTotal = Actor.Select(_connection, x => new { x.ActorId }).Count().Build().Execute();
                var customersTotal = Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().Execute();
                connection.Close();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);
            }
        }

        [Fact]
        public void Execute_with_connection_Throw_exeception()
        {
            Assert.Throws<ArgumentNullException>(() => BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_actors).Execute(null));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync();

            var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
            var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

            Assert.True(bulkcopy > 0);
            Assert.True(actorTotal > beforeActorTotal);
            Assert.True(customersTotal > beforeCustomersTotal);
        }

        [Fact]
        public async Task ExecuteAsync_with_connection()
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                await connection.OpenAsync();

                var beforeActorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var beforeCustomersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();

                var bulkcopy = await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).Copy(_actors).ExecuteAsync(connection);

                var actorTotal = await Actor.Select(_connection, x => new { x.ActorId }).Count().Build().ExecuteAsync();
                var customersTotal = await Customer.Select(_connection, x => new { x.CustomerId }).Count().Build().ExecuteAsync();
                connection.Close();

                Assert.True(bulkcopy > 0);
                Assert.True(actorTotal > beforeActorTotal);
                Assert.True(customersTotal > beforeCustomersTotal);
                
            }
        }

        [Fact]
        public async Task ExecuteAsync_with_connection_Throw_exeception()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await BulkCopyFactory.Create(CONNECTIONSTRING).Copy(_customers).ExecuteAsync(null));
        }
    }
}