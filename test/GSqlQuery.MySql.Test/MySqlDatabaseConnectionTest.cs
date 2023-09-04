using GSqlQuery.Runner;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseConnectionTest
    {
        public MySqlDatabaseConnectionTest()
        {
            Helper.CreateDatatable();
        }

        [Fact]
        public void Create_MySqlDatabaseConnection()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
                Assert.NotNull(result);
        }

        [Fact]
        public void Dispose()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                result.Dispose();
                Assert.Equal(ConnectionState.Closed, result.State);
            }
        }

        [Fact]
        public void Open_and_closed_connection()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                Assert.Equal(ConnectionState.Open, result.State);
                result.Close();
                Assert.Equal(ConnectionState.Closed, result.State);
            }
        }

        [Fact]
        public async Task Open_and_closed_async_connection()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                Assert.Equal(ConnectionState.Open, result.State);
                await result.CloseAsync();
                Assert.Equal(ConnectionState.Closed, result.State);
            }
        }

        [Fact]
        public async Task Open_and_closed_async_connection_with_cancelltoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync(token);
                Assert.Equal(ConnectionState.Open, result.State);
                await result.CloseAsync(token);
                Assert.Equal(ConnectionState.Closed, result.State);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_close()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync(token);
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await result.CloseAsync(token));
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_open()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await result.OpenAsync(token));
            }
        }

        [Fact]
        public void GetDbCommand()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                var command = result.GetDbCommand();
                Assert.NotNull(command);
                result.Close();
            }
        }

        [Fact]
        public void BeginTransaction()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                using (var transaction = result.BeginTransaction())
                {
                    Assert.NotNull(transaction);
                }
                result.Close();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public void BeginTransaction_with_isolationlevel(IsolationLevel isolationLevel)
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                using (var transaction = result.BeginTransaction(isolationLevel))
                {
                    Assert.Equal(isolationLevel, transaction.IsolationLevel);
                }
                result.Close();
            }

        }

        [Fact]
        public async Task BeginTransaction_async()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await result.BeginTransactionAsync())
                {
                    Assert.NotNull(transaction);
                }
                await result.CloseAsync();
            }
        }

        [Fact]
        public async Task BeginTransaction_async_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await result.BeginTransactionAsync(token))
                {
                    Assert.NotNull(transaction);
                }
                await result.CloseAsync();
            }

        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_beginTransaction_async()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await result.BeginTransactionAsync(token));
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task BeginTransaction_async_with_isolationlevel(IsolationLevel isolationLevel)
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await result.BeginTransactionAsync(isolationLevel))
                    Assert.Equal(isolationLevel, transaction.IsolationLevel);
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task BeginTransaction_async_with_isolationlevel_and_cancellationtoken(IsolationLevel isolationLevel)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await result.BeginTransactionAsync(isolationLevel, token))
                    Assert.NotNull(transaction);
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task Throw_exception_if_Cancel_token_on_beginTransaction_async_with_isolationlevel(IsolationLevel isolationLevel)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await result.BeginTransactionAsync(isolationLevel, token));
                await result.CloseAsync();
            }

        }

        [Fact]
        public void ITransaction_BeginTransaction()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                using (var transaction = ((IConnection)result).BeginTransaction())
                    Assert.NotNull(transaction);
                result.Close();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public void ITransaction_BeginTransaction_with_isolationlevel(IsolationLevel isolationLevel)
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                using (var transaction = ((IConnection)result).BeginTransaction(isolationLevel))
                    Assert.Equal(isolationLevel, transaction.IsolationLevel);
                result.Close();
            }
        }

        [Fact]
        public async Task ITransaction_BeginTransaction_async()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await ((IConnection)result).BeginTransactionAsync())
                    Assert.NotNull(transaction);
                await result.CloseAsync();
            }
        }

        [Fact]
        public async Task ITransaction_BeginTransaction_async_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await ((IConnection)result).BeginTransactionAsync(token))
                    Assert.NotNull(transaction);
                await result.CloseAsync();
            }
        }

        [Fact]
        public async Task ITransaction_Throw_exception_if_Cancel_token_on_beginTransaction_async()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await ((IConnection)result).BeginTransactionAsync(token));
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task ITransaction_BeginTransaction_async_with_isolationlevel(IsolationLevel isolationLevel)
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await ((IConnection)result).BeginTransactionAsync(isolationLevel))
                    Assert.Equal(isolationLevel, transaction.IsolationLevel);
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task ITransaction_BeginTransaction_async_with_isolationlevel_and_cancellationtoken(IsolationLevel isolationLevel)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                using (var transaction = await ((IConnection)result).BeginTransactionAsync(isolationLevel, token))
                    Assert.NotNull(transaction);
                await result.CloseAsync();
            }
        }

        [Theory]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.Serializable)]
        public async Task ITransaction_Throw_exception_if_Cancel_token_on_beginTransaction_async_with_isolationlevel(IsolationLevel isolationLevel)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                await result.OpenAsync();
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await ((IConnection)result).BeginTransactionAsync(isolationLevel, token));
                await result.CloseAsync();
            }
        }

        [Fact]
        public void RemoveTransaction()
        {
            using (MySqlDatabaseConnection result = new MySqlDatabaseConnection(Helper.ConnectionString))
            {
                result.Open();
                using (var transaction = result.BeginTransaction())
                    result.RemoveTransaction(transaction);
                result.Close();
            }
        }
    }
}