using GSqlQuery.MySql.Test.Data.Table;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GSqlQuery.MySql.Test.Extensions
{
    [Collection("GlobalTestServer")]
    public class LimitQueryExtensionTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public LimitQueryExtensionTest()
        {
            _connectionOptions = new MySqlConnectionOptions(GlobalFixture.CONNECTIONSTRING, new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void borrar_despues()
        {
            var connectionOptions = new MySqlConnectionOptions(GlobalFixture.CONNECTIONSTRING, new MySqlDatabaseManagementEvents());
            var result = Actor.Select(connectionOptions).Build().Execute();
            Assert.NotNull(result);

            //var result = Actor.Select(_connectionOptions)
            //                          .InnerJoin<Film_Actor>()
            //                          .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
            //                          .InnerJoin<Film>()
            //                          .Equal(x => x.Table2.FilmId, x => x.Table3.FilmId)
            //                          .Where()
            //                          .Equal(x => x.Table1.ActorId, 1)
            //                          .Limit(0, 5)
            //                          .Build().Execute();

            //Assert.NotNull(result);
        }

        [Fact]
        public void Limit_by_select()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` LIMIT 0,5;";

            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_by_select_and_where()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` WHERE `sakila`.`actor`.`last_name` IS NOT NULL LIMIT 0,5;";
            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).Where().IsNotNull(x => x.LastName).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }


        [Fact]
        public void Limit_by_select_and_orderby()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` ORDER BY `sakila`.`actor`.`actor_id` ASC LIMIT 0,5;";
            var result = Actor.Select(new QueryOptions(_connectionOptions.Formats)).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_execute_by_select()
        {
            var result = Address.Select(_connectionOptions).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Limit_execute_by_select_and_where()
        {
            var result = Actor.Select(_connectionOptions, x => new {x.LastName}).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public void Limit_execute_by_select_and_orderby()
        {
            var result = Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Limit_execute_with_connection_by_select()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Actor.Select(_connectionOptions).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Limit_execute_with_connection_by_select_and_where()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Address.Select(_connectionOptions).Where().IsNotNull(x => x.AddressId).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Limit_execute_with_connection_by_select_and_orderby()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                var result = Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Throw_exeception_when_connection_is_null()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                Assert.Throws<ArgumentNullException>(() => Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().Execute(null));
            }
        }

        [Fact]
        public async Task Limit_executeasync_by_select()
        {
            var result = await Actor.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_executeasync_by_select_and_where()
        {
            var result = await Address.Select(_connectionOptions).Where().IsNotNull(x => x.AddressId).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public async Task Limit_executeasync_by_select_and_orderby()
        {
            var result = await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_executeasync_with_token_by_select()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Address.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_executeasync_with_token_by_select_and_where()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Limit_executeasync_with_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task Throw_exeception_when_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token));
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_by_select()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_by_select_and_where()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_by_select_and_orderby()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Throw_exeception_when_connection_is_null_executeasync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null));
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_and_token_by_select()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_and_token_by_select_and_where()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Limit_executeasync_with_connection_and_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => new { x.AddressId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Throw_exeception_with_connection_and_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                cancellationTokenSource.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token));
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => new { x.ActorId }, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null, token));
            }
        }

        [Fact]
        public void Limit_executeasync_in_two_Join_with_where()
        {
            var result = Actor.Select(_connectionOptions)
                                      .InnerJoin<Film_Actor>()
                                      .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
                                      .Where()
                                      .Equal(x => x.Table1.ActorId, 1)
                                      .Limit(0, 5)
                                      .Build().Execute();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Limit_executeasync_in_two_Join_with_whereAsync()
        {
            var result = await Actor.Select(_connectionOptions)
                                      .InnerJoin<Film_Actor>()
                                      .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
                                      .Where()
                                      .Equal(x => x.Table1.ActorId, 1)
                                      .Limit(0,5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Limit_executeasync_in_two__Join()
        {
            var result = await Address.Select(_connectionOptions)
                                      .InnerJoin<City>()
                                      .Equal(x => x.Table1.CityId, x => x.Table2.CityId)
                                      .Limit(0, 5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Limit_executeasync_in_three_Join_with_where()
        {
            var result = await Actor.Select(_connectionOptions)
                                      .InnerJoin<Film_Actor>()
                                      .Equal(x => x.Table1.ActorId, x => x.Table2.ActorId)
                                      .InnerJoin<Film>()
                                      .Equal(x => x.Table2.FilmId, x => x.Table3.FilmId)
                                      .Where()
                                      .Equal(x => x.Table1.ActorId, 1)
                                      .Limit(0, 5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Limit_executeasync_in_three__Join()
        {
            var result = await Address.Select(_connectionOptions)
                                      .InnerJoin<City>()
                                      .Equal(x => x.Table1.CityId, x => x.Table2.CityId)
                                      .InnerJoin<Store>()
                                      .Equal(x => x.Table1.AddressId, x => x.Table3.AddressId)
                                      .Limit(0, 5)
                                      .Build().ExecuteAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}