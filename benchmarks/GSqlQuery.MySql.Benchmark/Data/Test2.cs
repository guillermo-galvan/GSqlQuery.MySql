using System;

namespace GSqlQuery.MySql.Benchmark.Data
{
    [Table("GSQLQuery", "test2")]
    public class Test2 : EntityExecute<Test2>
    {
        [Column("Id", Size = 20, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        public decimal? Money { get; set; }

        public bool? IsBool { get; set; }

        public DateTime? Time { get; set; }

        public Test2()
        { }

        public Test2(long id, decimal? money, bool? isBool, DateTime? time)
        {
            Id = id;
            Money = money;
            IsBool = isBool;
            Time = time;
        }

        public override string ToString()
        {
            return $"Id: {Id},Money: {Money}, IsBool:{IsBool}, Time:{Time}";
        }
    }
}