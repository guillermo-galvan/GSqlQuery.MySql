using GSqlQuery.MySql.Test.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test.Extensions
{
    public class LimitQueryExtensionTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public LimitQueryExtensionTest()
        {
            Helper.CreateDatatable();
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
        }

        [Fact]
        public void Limit_by_select()
        {
            var text = "SELECT `GSQLQuery`.`test1`.`idTest1`,`GSQLQuery`.`test1`.`Money`,`GSQLQuery`.`test1`.`Nombre`,`GSQLQuery`.`test1`.`GUID`,`GSQLQuery`.`test1`.`URL` FROM `GSQLQuery`.`test1` LIMIT 0,5;";

            var result = Test1.Select(_connectionOptions.Statements).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_by_select_and_where()
        {
            var text = "SELECT `GSQLQuery`.`test1`.`idTest1`,`GSQLQuery`.`test1`.`Money`,`GSQLQuery`.`test1`.`Nombre`,`GSQLQuery`.`test1`.`GUID`,`GSQLQuery`.`test1`.`URL` FROM `GSQLQuery`.`test1` WHERE `GSQLQuery`.`test1`.`idTest1` IS NOT NULL LIMIT 0,5;";
            var result = Test1.Select(_connectionOptions.Statements).Where().IsNotNull(x => x.Id).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }


        [Fact]
        public void Limit_by_select_and_orderby()
        {
            var text = "SELECT `GSQLQuery`.`test1`.`idTest1`,`GSQLQuery`.`test1`.`Money`,`GSQLQuery`.`test1`.`Nombre`,`GSQLQuery`.`test1`.`GUID`,`GSQLQuery`.`test1`.`URL` FROM `GSQLQuery`.`test1` ORDER BY `GSQLQuery`.`test1`.`idTest1` ASC LIMIT 0,5;";
            var result = Test1.Select(_connectionOptions.Statements).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_execute_by_select()
        {
            var result = Test1.Select(_connectionOptions).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Limit_execute_by_select_and_where()
        {
            var result = Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public void Limit_execute_by_select_and_orderby()
        {
            var result = Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Limit_execute_with_connection_by_select()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Test1.Select(_connectionOptions).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Limit_execute_with_connection_by_select_and_where()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Limit_execute_with_connection_by_select_and_orderby()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Throw_exeception_when_connection_is_null()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                Assert.Throws<ArgumentNullException>(() => Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().Execute(null));
            }
        }

        [Fact]
        public async Task Limit_executeasync_by_select()
        {
            var result = await Test1.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Limit_executeasync_by_select_and_where()
        {
            var result = await Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public async void Limit_executeasync_by_select_and_orderby()
        {
            var result = await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_executeasync_with_token_by_select()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Test1.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Limit_executeasync_with_token_by_select_and_where()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Limit_executeasync_with_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Throw_exeception_when_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token));
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_by_select()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Limit_executeasync_with_connection_by_select_and_where()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Limit_executeasync_with_connection_by_select_and_orderby()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Throw_exeception_when_connection_is_null_executeasync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null));
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_and_token_by_select()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Limit_executeasync_with_connection_and_token_by_select_and_where()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).Where().IsNotNull(x => x.Id).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Limit_executeasync_with_connection_and_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Throw_exeception_with_connection_and_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                cancellationTokenSource.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token));
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await Test1.Select(_connectionOptions).OrderBy(x => x.Id, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null, token));
            }

        }
    }
}