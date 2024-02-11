using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using GSqlQuery.MySql.Benchmark.Data.Parameters;
using GSqlQuery.MySql.Benchmark.Data.Table;
using GSqlQuery.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GSqlQuery.MySql.Benchmark
{
    public class Program
    {
        protected static MySqlConnectionOptions _connectionOptions;
        protected static ServiceProvider _serviceCollection;
        protected static MySqlConnectionOptions _connectionOptionsServicesProvider;
        private static IEnumerable<long> _ids;

        public static void Main(string[] args)
        {
            _serviceCollection = new ServiceCollection()
            .AddScoped<ITransformTo<Actor>, Data.Transform.Actors>()
            .AddScoped<IGetParameterTypes<Actor>, Actors>()
           .BuildServiceProvider();
            _connectionOptions = CreateTable.GetConnectionOptions();
            _connectionOptionsServicesProvider = CreateTable.GetConnectionOptions(_serviceCollection);
            _ids = Enumerable.Range(0, 250).Select(x => (long)x);

            var a = CreateTable.GetConnectionString();
            Console.WriteLine(a);

            var v = Actor.Select(_connectionOptionsServicesProvider, x => new { x.ActorId, x.FirstName }).Build().Execute();

            Console.WriteLine(v);

            //IConfig config = DefaultConfig.Instance;

            //config = config
            //    .AddDiagnoser(MemoryDiagnoser.Default)
            //    .AddColumn(StatisticColumn.OperationsPerSecond);

            //var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

            //Console.WriteLine(summary);

        }

    }
}