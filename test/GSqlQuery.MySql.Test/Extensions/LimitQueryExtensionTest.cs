﻿using GSqlQuery.MySql.Test.Data.Table;
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
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString, new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void Limit_by_select()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` LIMIT 0,5;";

            var result = Actor.Select(_connectionOptions.Formats).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void Limit_by_select_and_where()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` WHERE `sakila`.`actor`.`last_name` IS NOT NULL LIMIT 0,5;";
            var result = Actor.Select(_connectionOptions.Formats).Where().IsNotNull(x => x.LastName).Limit(0, 5).Build();
            Assert.NotNull(result);
            Assert.Equal(text, result.Text);
        }


        [Fact]
        public void Limit_by_select_and_orderby()
        {
            var text = "SELECT `sakila`.`actor`.`actor_id`,`sakila`.`actor`.`first_name`,`sakila`.`actor`.`last_name`,`sakila`.`actor`.`last_update` FROM `sakila`.`actor` ORDER BY `sakila`.`actor`.`actor_id` ASC LIMIT 0,5;";
            var result = Actor.Select(_connectionOptions.Formats).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build();
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
            var result = Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().Execute();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public void Limit_execute_by_select_and_orderby()
        {
            var result = Address.Select(_connectionOptions).OrderBy(x => x.AddressId, OrderBy.ASC).Limit(0, 5).Build().Execute();
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
                var result = Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().Execute(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public void Throw_exeception_when_connection_is_null()
        {
            using (var connection = _connectionOptions.DatabaseManagement.GetConnection())
            {
                Assert.Throws<ArgumentNullException>(() => Address.Select(_connectionOptions).OrderBy(x => x.AddressId, OrderBy.ASC).Limit(0, 5).Build().Execute(null));
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
        public async void Limit_executeasync_by_select_and_where()
        {
            var result = await Address.Select(_connectionOptions).Where().IsNotNull(x => x.AddressId).Limit(0, 5).Build().ExecuteAsync();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public async void Limit_executeasync_by_select_and_orderby()
        {
            var result = await Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync();
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
        public async void Limit_executeasync_with_token_by_select_and_where()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Limit_executeasync_with_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            var result = await Address.Select(_connectionOptions).OrderBy(x => x.AddressId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void Throw_exeception_when_cancel_token()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(token));
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
        public async void Limit_executeasync_with_connection_by_select_and_where()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Actor.Select(_connectionOptions).Where().IsNotNull(x => x.ActorId).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async void Limit_executeasync_with_connection_by_select_and_orderby()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => x.AddressId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection);
                Assert.NotNull(result);
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task Throw_exeception_when_connection_is_null_executeasync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null));
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
        public async void Limit_executeasync_with_connection_and_token_by_select_and_where()
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
        public async void Limit_executeasync_with_connection_and_token_by_select_and_orderby()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                var result = await Address.Select(_connectionOptions).OrderBy(x => x.AddressId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token);
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
                await Assert.ThrowsAsync<OperationCanceledException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(connection, token));
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await Actor.Select(_connectionOptions).OrderBy(x => x.ActorId, OrderBy.ASC).Limit(0, 5).Build().ExecuteAsync(null, token));
            }

        }
    }
}