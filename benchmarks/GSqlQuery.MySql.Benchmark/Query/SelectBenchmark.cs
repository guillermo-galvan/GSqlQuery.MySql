using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using GSqlQuery.MySql.Benchmark.Data.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class SelectBenchmark : BenchmarkBase
    {
        public SelectBenchmark()
        {
            CreateTable.Create();
            int count = Actor.Select(_connectionOptions, x => x.ActorId).Count().Build().Execute();
            Console.WriteLine("Init Initialize {1} 2 {0}", count, typeof(Actor));
        }
    }

    public class Select : SelectBenchmark
    {
        private readonly IEnumerable<long> _ids;
        public Select()
        {
            _ids = Enumerable.Range(0, 250).Select(x => (long)x);
        }

        [Params(true, false)]
        public bool IsServicesProvider { get; set; }


        [Benchmark]
        public async Task<int> Select_All()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Build() : Actor.Select(_connectionOptions).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_Many_Columns_true()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new {x.ActorId, x.FirstName}).Build() : 
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_All_Columns_With_Where()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions).Where().In(x => x.ActorId, _ids).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }

        [Benchmark]
        public async Task<int> Select_Many_Columns_With_Where()
        {
            var query = IsServicesProvider ? Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build() :
                                             Actor.Select(_connectionOptions, x => new { x.ActorId, x.FirstName }).Where().In(x => x.ActorId, _ids).Build();
            var result = Async ? await query.ExecuteAsync() : query.Execute();
            return result.Count();
        }
    }
}