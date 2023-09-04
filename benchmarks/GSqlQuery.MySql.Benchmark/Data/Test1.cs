namespace GSqlQuery.MySql.Benchmark.Data
{
    [Table("GSQLQuery", "test1")]
    public class Test1 : EntityExecute<Test1>
    {
        [Column("idTest1")]
        public long Id { get; set; }

        [Column("Money")]
        public decimal? Money { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("GUID")]
        public string GUID { get; set; }

        [Column("URL")]
        public string URL { get; set; }

        public Test1()
        {

        }

        public Test1(long id, decimal? money, string nombre, string guid, string url)
        {
            Id = id;
            Money = money;
            Nombre = nombre;
            GUID = guid;
            URL = url;
        }

        public override string ToString()
        {
            return $"Id: {Id},Money: {Money},Nombre:{Nombre}, GUID:{GUID}, URL:{URL}";
        }

    }
}