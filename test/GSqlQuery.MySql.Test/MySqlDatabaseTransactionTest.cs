using GSqlQuery.MySql.Test.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseTransactionTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseTransactionTest()
        {
            Helper.CreateDatatable();
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
        }


        [Fact]
        public void Commit()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    transaction.Commit();
                    connection.Close();
                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }

        }

        [Fact]
        public async Task CommitAsync()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }
        }

        [Fact]
        public async Task CommitAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    await transaction.CommitAsync(token);
                    await connection.CloseAsync(token);
                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).Build().Execute().Any();
                    Assert.True(isExists);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_CommitAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await transaction.CommitAsync(token));
                }
            }
        }

        [Fact]
        public void Rollback()
        {
            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var result = test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    transaction.Rollback();
                    connection.Close();

                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).Build().Execute().Any();
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task RollbackAsync()
        {
            Test2 test = new Test2() { IsBool = true, Money = Convert.ToDecimal(200d + new Random().NextDouble()), Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection);
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).AndEqual(x => x.Money, test.Money).Build().Execute().Any();
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task RollbackAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test2 test = new Test2() { IsBool = true, Money = Convert.ToDecimal(100d + new Random().NextDouble()), Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    await transaction.RollbackAsync(token);
                    var isExists = Test2.Select(_connectionOptions).Where().Equal(x => x.Id, result.Id).AndEqual(x => x.Money, test.Money).Build().Execute(connection).Any();
                    await connection.CloseAsync(token);
                    Assert.False(isExists);
                }
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_RollbackAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test2 test = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    var result = await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection, token);
                    source.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => await transaction.RollbackAsync(token));
                }
            }
        }
    }
}