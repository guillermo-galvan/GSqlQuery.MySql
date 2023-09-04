using GSqlQuery.MySql.Test.Data;
using GSqlQuery.Runner;
using GSqlQuery.Runner.Extensions;
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
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
        }

        [Fact]
        public void ExecuteNonQuery()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            int result = managment.ExecuteNonQuery(query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result > 0);
        }

        [Fact]
        public void ExecuteNonQuery_with_connection()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                int result = managment.ExecuteNonQuery(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public void IConnection_executeNonQuery_with_connection()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                int result = managment.ExecuteNonQuery(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteNonQueryAsync()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            int result = await managment.ExecuteNonQueryAsync(query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            int result = await managment.ExecuteNonQueryAsync(query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_executeNonQueryAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteNonQueryAsync(query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_connection()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_executeNonQueryAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }

        [Fact]
        public async Task IConnection_executeNonQueryAsync_with_connection()
        {
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task IConnection_executeNonQueryAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                int result = await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_executeNonQueryAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Test1 test1 = new Test1() { Id = 1, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var query = test1.Update(_connectionOptions, x => new { x.GUID, x.Money }).Where().Equal(x => x.Id, 1).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteNonQueryAsync(connection, query, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }

        [Fact]
        public void ExecuteReader()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = managment.ExecuteReader<Test1>(query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result.Any());
        }

        [Fact]
        public void ExecuteReader_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteReader<Test1>(connection, query, classOptions.PropertyOptions, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result.Any());
            }
        }

        [Fact]
        public void IConnection_executeReader_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteReader<Test1>(connection, query, classOptions.PropertyOptions, query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task ExecuteReaderAsync()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = await managment.ExecuteReaderAsync<Test1>(query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result.Any());
        }

        [Fact]
        public async Task ExecuteReaderAsync_with_cancellation_token()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = await managment.ExecuteReaderAsync<Test1>(query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteReaderAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteReaderAsync<Test1>(query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
        }

        [Fact]
        public async Task ExecuteReader_Async_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task ExecuteReader_Async_with_cancellation_token_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteReaderAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                    query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }

        [Fact]
        public async Task IConnection_ExecuteReader_Async_with_connection()
        {
            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task IConnection_ExecuteReader_Async_with_cancellation_token_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result.Any());
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteReaderAsync_with_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var classOptions = ClassOptionsFactory.GetClassOptions(typeof(Test1));
            var query = Test1.Select(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteReaderAsync<Test1>(connection, query, classOptions.PropertyOptions,
                    query.GetParameters<Test1, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }

        [Fact]
        public void ExecuteScalar()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = managment.ExecuteScalar<long>(query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result > 0);
        }

        [Fact]
        public void ExecuteScalar_with_connection()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteScalar<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public void IConnection_executeScalar_with_connection()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = managment.ExecuteScalar<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsync()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = await managment.ExecuteScalarAsync<long>(query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_cancellationtoken()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            var result = await managment.ExecuteScalarAsync<long>(query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteScalarAsync()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await managment.ExecuteScalarAsync<long>(query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_connection()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_ExecuteScalarAsync_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }

        [Fact]
        public async Task IConnection_ExecuteScalarAsync_with_connection()
        {
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement));
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task IConnection_ExecuteScalarAsync_with_cancellationtoken_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                var result = await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public async Task Throw_exception_if_Cancel_token_on_IConnection_ExecuteScalarAsync_and_connection()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Test2 test1 = new Test2() { IsBool = true, Money = 100m, Time = DateTime.Now };
            var query = test1.Insert(_connectionOptions).Build();
            var managment = new MySqlDatabaseManagement(Helper.ConnectionString);
            using (IConnection connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync(token))
            {
                source.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await managment.ExecuteScalarAsync<long>(connection, query, query.GetParameters<Test2, MySqlDatabaseConnection>(_connectionOptions.DatabaseManagement), token));
            }
        }
    }
}