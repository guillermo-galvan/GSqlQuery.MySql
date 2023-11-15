using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GSqlQuery.MySql.Benchmark.Data.Parameters;
using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace GSqlQuery.MySql.Benchmark.Query
{
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [SimpleJob(RuntimeMoniker.Net462)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public abstract class BenchmarkBase
    {
        protected readonly MySqlConnectionOptions _connectionOptions;
        protected readonly ServiceProvider _serviceCollection;
        protected readonly MySqlConnectionOptions _connectionOptionsServicesProvider;
        public BenchmarkBase()
        {
            _serviceCollection = new ServiceCollection()
            .AddScoped<ITransformTo<Actor>, Data.Transform.Actors>()
            .AddScoped<IGetParameterTypes<Actor>, Actors>()
           .BuildServiceProvider();
            _connectionOptions = CreateTable.GetConnectionOptions();
            _connectionOptionsServicesProvider = CreateTable.GetConnectionOptions(_serviceCollection);
        }

        [Params(true, false)]
        public bool Async { get; set; }
    }
}