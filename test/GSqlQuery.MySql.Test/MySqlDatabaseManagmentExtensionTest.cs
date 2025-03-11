using GSqlQuery.MySql.Test.Data.Table;
using MySql.Data.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    [Collection("GlobalTestServer")]
    public class MySqlDatabaseManagementExtensionTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseManagementExtensionTest()
        {
            _connectionOptions = new MySqlConnectionOptions(GlobalFixture.CONNECTIONSTRING, new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void ExecuteWithTransaction()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var result = actor.Insert(_connectionOptions).Build().ExecuteWithTransaction();
            Assert.NotNull(result);
            Assert.True(result.ActorId > 0);
        }

        [Fact]
        public void ExecuteWithTransaction_and_parameters()
        {
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = address.Insert(_connectionOptions).Build().ExecuteWithTransaction(transaction);
                    transaction.Commit();
                    connection.Close();
                    Assert.NotNull(result);
                    Assert.True(result.AddressId > 0);
                }
            }

        }

        [Fact]
        public async Task ExecuteWithTransactionAsync()
        {
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            var result = await actor.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync();
            Assert.NotNull(result);
            Assert.True(result.ActorId > 0);
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            var result = await address.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(token);
            Assert.NotNull(result);
            Assert.True(result.AddressId > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteWithTransactionAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await actor.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(token));
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_and_transaction()
        {
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await address.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    Assert.NotNull(result);
                    Assert.True(result.AddressId > 0);
                }
            }
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_with_cancellationtoken_and_transaction()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Actor actor = new Actor(0, "PENELOPE", "PENELOPE", DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await actor.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction, token);
                    await transaction.CommitAsync(token);
                    await connection.CloseAsync(token);
                    Assert.NotNull(result);
                    Assert.True(result.ActorId > 0);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteWithTransactionAsync_and_parameters()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Address address = new Address(0, "47 MySakila Drive", null, "Alberta", 300, string.Empty, string.Empty, new MySqlGeometry(153.1408538, -27.6333361), DateTime.Now);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await address.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction, token));
                }
            }
        }
    }
}