using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace GSqlQuery.MySql.Benchmark.Query
{
    [SimpleJob(RuntimeMoniker.Net70, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net462)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public abstract class BenchmarkBase
    {
        protected readonly MySqlConnectionOptions _connectionOptions;

        public BenchmarkBase()
        {
            _connectionOptions = CreateTable.GetConnectionOptions();
        }

        [Params(true, false)]
        public bool Async { get; set; }
    }
}