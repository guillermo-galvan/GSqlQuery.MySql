using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class UpdateBenchmark : BenchmarkBase
    {
        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            CreateTable.Create();
            Test2.Delete(_connectionOptions).Build().Execute();
            for (int i = 0; i < 4; i++)
            {
                CreateTable.CreateData();
            }
        }

        [IterationSetup]
        public virtual void InitializeTest()
        {
            int count = Test2.Select(_connectionOptions, x => x.Id).Count().Build().Execute();
            Console.WriteLine("Init Initialize test 2 {0}", count);
        }
    }

    public class Update : UpdateBenchmark
    {
        [Benchmark]
        public async Task<int> Update_Bool()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            test.IsBool = true;
            var query = test.Update(_connectionOptions, x => x.IsBool).Where().Equal(x => x.IsBool, false).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_DateTime()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            test.Time = DateTime.Now;
            var query = test.Update(_connectionOptions, x => x.Time).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_Decimal()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            test.Money = 1263.36m;
            var query = test.Update(_connectionOptions, x => x.Money).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_AllColumns()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            test.Money = 1263.36m;
            test.Time = DateTime.Now;
            test.IsBool = true;
            var query = test.Update(_connectionOptions, x => new { x.Money, x.Time, x.IsBool }).Where().In(x => x.Id, Enumerable.Range(1, 1000).Select(x => (long)x)).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }

        [Benchmark]
        public async Task<int> Update_AllColumns_by_Id()
        {
            Test2 test = new Test2() { IsBool = false, Money = 200m, Time = DateTime.Now };
            test.Money = 1263.36m;
            test.Time = DateTime.Now;
            test.IsBool = true;
            var query = test.Update(_connectionOptions, x => new { x.Money, x.Time, x.IsBool }).Where().Equal(x => x.Id, 1).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }
    }
}