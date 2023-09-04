using BenchmarkDotNet.Attributes;
using GSqlQuery.MySql.Benchmark.Data;
using GSqlQuery.MySql.BulkCopy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GSqlQuery.MySql.Benchmark.Query
{
    public abstract class BulkCopyBenchmark<T> : BenchmarkBase where T : class, new()
    {
        protected readonly string _connectionString;

        protected readonly Dictionary<string, Queue<T>> _dataCollection = new Dictionary<string, Queue<T>>();
        protected Queue<T> Data { get; set; }

        public string FileName { get; set; }

        public BulkCopyBenchmark()
        {
            _connectionString = CreateTable.ConnectionString + "AllowLoadLocalInfile=true;AllowUserVariables=True;";
        }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            EntityExecute<T>.Delete(_connectionOptions).Build().Execute();
            CreateTable.Create();
        }

        [IterationSetup]
        public virtual void InitializeTest()
        {
            if (_dataCollection.ContainsKey(FileName))
            {
                Data = _dataCollection[FileName];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(FileName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Queue<T> list = new Queue<T>();

                if (!string.IsNullOrEmpty(FileName))
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", FileName);
                    foreach (var item in File.ReadAllLines(path))
                    {
                        string[] columns = item.Split(',');
                        list.Enqueue(GetValue(columns));
                    }
                }

                _dataCollection[FileName] = list;
                Data = list;
            }
        }

        protected abstract T GetValue(string[] columns);

        [Benchmark]
        public async Task<int> BulkCopyExecute()
        {
            var query = BulkCopyFactory.Create(_connectionString).Copy(Data);
            return Async ? await query.ExecuteAsync() : query.Execute();
        }
    }

    public class BulkCopyTest1 : BulkCopyBenchmark<Test1>
    {
        public BulkCopyTest1() : base()
        {
            FileName = "Test1_10000.csv";
        }

        protected override Test1 GetValue(string[] columns)
        {
            return new Test1() { Id = Convert.ToInt64(columns[0]), Money = Convert.ToDecimal(columns[1]), Nombre = columns[2], GUID = columns[3], URL = columns[4] };
        }
    }

    public class BulkCopyTest2 : BulkCopyBenchmark<Test2>
    {
        public BulkCopyTest2() : base()
        {
            FileName = "Test2_10000.csv";
        }

        protected override Test2 GetValue(string[] columns)
        {
            return new Test2() { Money = Convert.ToDecimal(columns[0]), IsBool = Convert.ToBoolean(columns[1] == "1"), Time = Convert.ToDateTime(columns[2]) };
        }
    }
}