using GSqlQuery.MySql.Test.Data.Table;
using GSqlQuery.Runner;
using MySql.Data.Types;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseManagementTest()
        {
            Helper.CreateDatatable();
            _connectionOptions = new MySqlConnectionOptions(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void ExecuteNonQuery()
        {
            Actor actor = new Actor(1, "PENELOPE", "PENELOPE", DateTime.Now);

            var query = actor.Update(_connectionOptions, x => new { x.LastUpdate, x.LastName }).Where().Equal(x => x.ActorId, actor.ActorId).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            int result = managment.ExecuteNonQuery(query);
            Assert.True(result > 0);
        }

        [Fact]
        public void ExecuteNonQuery_with_connection()
        {
            var query = Address.Update(_connectionOptions, x => x.Location, new MySqlGeometry(153.1408538, -27.6333361))
                               .Set(x => x.LastUpdate, DateTime.Now)
                               .Where()
                               .Equal(x => x.AddressId, 1)
                               .Build();

            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                int result = managment.ExecuteNonQuery(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public void IConnection_executeNonQuery_with_connection()
        {
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);

            var query = address.Update(_connectionOptions, x => new { x.Location, x.LastUpdate }).Where().Equal(x => x.AddressId, 3).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                int result = managment.ExecuteNonQuery(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteNonQueryAsync()
        {
            var query = Actor.Update(_connectionOptions, x => x.LastUpdate, DateTime.Now).Where().Equal(x => x.ActorId, 2).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            int result = await managment.ExecuteNonQueryAsync(query);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);

            var query = address.Update(_connectionOptions, x => new { x.Location, x.LastUpdate }).Where().Equal(x => x.AddressId, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            int result = await managment.ExecuteNonQueryAsync(query, token);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_executeNonQueryAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var query = Actor.Update(_connectionOptions, x => x.LastUpdate, DateTime.Now).Where().Equal(x => x.ActorId, 2).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteNonQueryAsync(query, token));
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_connection()
        {
            var query = Address.Update(_connectionOptions, x => x.Location, new MySqlGeometry(153.1408538, -27.6333361))
                             .Set(x => x.LastUpdate, DateTime.Now)
                             .Where()
                             .Equal(x => x.AddressId, 1)
                             .Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var query = Actor.Update(_connectionOptions, x => x.LastUpdate, DateTime.Now).Where().Equal(x => x.ActorId, 2).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_executeNonQueryAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var query = Address.Update(_connectionOptions, x => x.Location, new MySqlGeometry(153.1408538, -27.6333361))
                              .Set(x => x.LastUpdate, DateTime.Now)
                              .Where()
                              .Equal(x => x.AddressId, 1)
                              .Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteNonQueryAsync(connection, query, token));
            }
        }

        [Fact]
        public async Task IConnection_executeNonQueryAsync_with_connection()
        {
            var query = Actor.Update(_connectionOptions, x => x.LastUpdate, DateTime.Now).Where().Equal(x => x.ActorId, 2).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task IConnection_executeNonQueryAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var query = Address.Update(_connectionOptions, x => x.Location, new MySqlGeometry(153.1408538, -27.6333361))
                              .Set(x => x.LastUpdate, DateTime.Now)
                              .Where()
                              .Equal(x => x.AddressId, 1)
                              .Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_executeNonQueryAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var query = Actor.Update(_connectionOptions, x => x.LastUpdate, DateTime.Now).Where().Equal(x => x.ActorId, 2).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteNonQueryAsync(connection, query, token));
            }
        }

        [Fact]
        public void ExecuteReader()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());

            var result = managment.ExecuteReader(query, classOptions.PropertyOptions);
            Assert.True(result.Any());
        }

        [Fact]
        public void ExecuteReader_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions, x => new { x.Location, x.AddressId}).Build();

            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteReader(connection, query, classOptions.PropertyOptions);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public void IConnection_executeReader_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteReader(connection, query, classOptions.PropertyOptions);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task ExecuteReaderAsync()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
            var result = await managment.ExecuteReaderAsync(query, classOptions.PropertyOptions);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task ExecuteReaderAsync_with_cancellation_token()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            var result = await managment.ExecuteReaderAsync(query, classOptions.PropertyOptions, token);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteReaderAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteReaderAsync(query, classOptions.PropertyOptions, token));
        }

        [Fact]
        public async Task ExecuteReader_Async_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task ExecuteReader_Async_with_cancellation_token_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions, token);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteReaderAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions, token));
            }
        }

        [Fact]
        public async Task IConnection_ExecuteReader_Async_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task IConnection_ExecuteReader_Async_with_cancellation_token_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Actor));
            var query = Actor.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions, token);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteReaderAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Address));
            var query = Address.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteReaderAsync(connection, query, classOptions.PropertyOptions, token));
            }
        }

        [Fact]
        public void ExecuteScalar()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            var result = managment.ExecuteScalar<long>(query);
            Assert.True(result > 0);
        }

        [Fact]
        public void ExecuteScalar_with_connection()
        {
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteScalar<long>(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public void IConnection_executeScalar_with_connection()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteScalar<long>(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsync()
        {
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            var result = await managment.ExecuteScalarAsync<long>(query);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            var result = await managment.ExecuteScalarAsync<long>(query, token);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteScalarAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteScalarAsync<long>(query, token));
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_connection()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteScalarAsync_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteScalarAsync<long>(connection, query, token));
            }
        }

        [Fact]
        public async Task IConnection_ExecuteScalarAsync_with_connection()
        {
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task IConnection_ExecuteScalarAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var query = actor.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteScalarAsync_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(1, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var query = address.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.GetConnectionString());
            using (MySqlDatabaseConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteScalarAsync<long>(connection, query, token));
            }
        }
    }
}