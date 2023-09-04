using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class SelectBenchmark : BenchmarkBase
    {
        public SelectBenchmark()
        {
            GlobalSetup();
        }

        public virtual void GlobalSetup()
        {
            CreateTable.Create();

            int count = Test2.Select(_connectionOptions, x => x.Id).Count().Build().Execute();

            if (count < 100000)
            {
                Test2.Delete(_connectionOptions).Build().Execute();
                for (int i = 0; i < 100; i++)
                {
                    CreateTable.CreateData();
                }
            }

            count = Test2.Select(_connectionOptions, x => x.Id).Count().Build().Execute();
            Console.WriteLine("Init Initialize test 2 {0}", count);
        }
    }

    public class Select : SelectBenchmark
    {
        [Benchmark]
        public async Task<int> Select_All()
        {
            var query = Test2.Select(_connectionOptions).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_IsBool_true()
        {
            var query = Test2.Select(_connectionOptions).Where().Equal(x => x.IsBool, true).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_IsBool_false()
        {
            var query = Test2.Select(_connectionOptions).Where().Equal(x => x.IsBool, false).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_Between()
        {
            var query = Test2.Select(_connectionOptions).Where().Between(x => x.Time, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }
    }
}