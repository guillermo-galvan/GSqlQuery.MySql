using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using GSqlQuery.Runner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class InsertBenchmark : BenchmarkBase
    {
        public int LastId { get; set; } = 0;

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            CreateTable.Create();
            Test1.Delete(_connectionOptions).Build().Execute();
            Test2.Delete(_connectionOptions).Build().Execute();
        }

        [IterationSetup]
        public virtual void InitializeTest()
        {
            LastId = Test1.Select(_connectionOptions, x => x.Id).Count().Build().Execute();
            int count = Test2.Select(_connectionOptions, x => x.Id).Count().Build().Execute();
            Console.WriteLine("Init Initialize test 1 {0}, test 2 {1}", LastId, count);
        }
    }

    public class Single_Insert : InsertBenchmark
    {
        [Benchmark]
        public async Task<Test1> GenerateQuery_Test1()
        {
            Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            test.Id += 1;
            return Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute();
        }

        [Benchmark]
        public async Task<Test1> GenerateQueryTransaction_Test1()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
                    test.Id += 1;
                    var result = Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection) : test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<Test2> GenerateQuery_Test2()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            return Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute();
        }

        [Benchmark]
        public async Task<Test2> GenerateQueryTransaction_Test2()
        {
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
                    var result = Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection)
                                       : test.Insert(_connectionOptions).Build().Execute(transaction.Connection);
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return result;
                }
            }
        }
    }

    public class Many_Insert : InsertBenchmark
    {
        [Params(10, 100, 1000)]
        public int Rows { get; set; }

        [Benchmark]
        public async Task<List<Test1>> GenerateQuery_Test1()
        {
            List<Test1> result = new List<Test1>();
            Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            for (int i = 0; i < Rows; i++)
            {
                test.Id += 1;
                result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute());
            }

            return result;
        }

        [Benchmark]
        public async Task<List<Test1>> GenerateQueryTransaction_Test1()
        {
            List<Test1> result = new List<Test1>();

            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
                    for (int i = 0; i < Rows; i++)
                    {
                        test.Id += 1;
                        result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection) : test.Insert(_connectionOptions).Build().Execute(transaction.Connection));
                    }

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<List<Test2>> GenerateQuery_Test2()
        {
            List<Test2> result = new List<Test2>();
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };

            for (int i = 0; i < Rows; i++)
            {
                result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync() : test.Insert(_connectionOptions).Build().Execute());
            }

            return result;
        }

        [Benchmark]
        public async Task<List<Test2>> GenerateQueryTransaction_Test2()
        {
            List<Test2> result = new List<Test2>();
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
                    for (int i = 0; i < Rows; i++)
                    {
                        result.Add(Async ? await test.Insert(_connectionOptions).Build().ExecuteAsync(transaction.Connection)
                                         : test.Insert(_connectionOptions).Build().Execute(transaction.Connection));
                    }

                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return result;
                }
            }
        }
    }

    public class Many_Insert_Batch : InsertBenchmark
    {
        [Params(10, 100, 1000)]
        public int Rows { get; set; }

        [Benchmark]
        public async Task<int> Batch_GenerateQuery_Test1()
        {
            Console.WriteLine("Batch_GenerateQuery_Test1");
            Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
            var batch = Execute.BatchExecuteFactory(_connectionOptions);
            for (int i = 0; i < Rows; i++)
            {
                test.Id += 1;
                batch.Add(sb => test.Insert(sb).Build());
            }

            return Async ? await batch.ExecuteAsync() : batch.Execute();
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQueryTransaction_Test1()
        {
            Console.WriteLine("Batch_GenerateQueryTransaction_Test1");
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var batch = Execute.BatchExecuteFactory(_connectionOptions);

                    Test1 test = new Test1() { Id = LastId, GUID = Guid.NewGuid().ToString(), Money = 120m, Nombre = "Test", URL = "https://guillermo-galvan.com/" };
                    for (int i = 0; i < Rows; i++)
                    {
                        test.Id += 1;
                        batch.Add(sb => test.Insert(sb).Build());
                    }

                    int result = Async ? await batch.ExecuteAsync(transaction.Connection) : batch.Execute(transaction.Connection);

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQuery_Test2()
        {
            Console.WriteLine("Batch_GenerateQuery_Test2");
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            var batch = Execute.BatchExecuteFactory(_connectionOptions);
            for (int i = 0; i < Rows; i++)
            {
                batch.Add(sb => test.Insert(sb).Build());
            }

            return Async ? await batch.ExecuteAsync() : batch.Execute(); ;
        }

        [Benchmark]
        public async Task<int> Batch_GenerateQueryTransaction_Test2()
        {
            Console.WriteLine("Batch_GenerateQueryTransaction_Test2");
            using (var connection = await _connectionOptions.DatabaseManagement.GetConnectionAsync())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
                    var batch = Execute.BatchExecuteFactory(_connectionOptions);
                    for (int i = 0; i < Rows; i++)
                    {
                        batch.Add(sb => test.Insert(sb).Build());
                    }

                    int result = Async ? await batch.ExecuteAsync(transaction.Connection) : batch.Execute(transaction.Connection);

                    await transaction.CommitAsync();
                    await connection.CloseAsync();

                    return result;
                }
            }
        }
    }
}