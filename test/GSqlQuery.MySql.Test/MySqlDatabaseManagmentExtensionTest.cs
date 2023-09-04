using GSqlQuery.MySql.Test.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementExtensionTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseManagementExtensionTest()
        {
            Helper.CreateDatatable();
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
        }

        [Fact]
        public void ExecuteWithTransaction()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var result = test.Insert(_connectionOptions).Build().ExecuteWithTransaction();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public void ExecuteWithTransaction_and_parameters()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = test.Insert(_connectionOptions).Build().ExecuteWithTransaction(transaction);
                    transaction.Commit();
                    connection.Close();
                    Assert.NotNull(result);
                    Assert.True(result.Id > 0);
                }
            }

        }

        [Fact]
        public async Task ExecuteWithTransactionAsync()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var result = await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var result = await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(token);
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteWithTransactionAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(token));
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_and_transaction()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    Assert.NotNull(result);
                    Assert.True(result.Id > 0);
                }
            }
        }

        [Fact]
        public async Task ExecuteWithTransactionAsync_with_cancellationtoken_and_transaction()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction, token);
                    await transaction.CommitAsync(token);
                    await connection.CloseAsync(token);
                    Assert.NotNull(result);
                    Assert.True(result.Id > 0);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteWithTransactionAsync_and_parameters()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await test.Insert(_connectionOptions).Build().ExecuteWithTransactionAsync(transaction, token));
                }
            }
        }
    }
}