using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using System;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class DeleteBenchmark : BenchmarkBase
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

    public class Delete : DeleteBenchmark
    {
        [Params(true, false)]
        public bool IsBool { get; set; }

        [Benchmark]
        public async Task<int> Delete_Bool()
        {
            var query = Test2.Delete(_connectionOptions).Where().Equal(x => x.IsBool, IsBool).Build();
            return Async ? await query.ExecuteAsync() : query.Execute();
        }
    }
}