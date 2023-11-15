namespace GSqlQuery.MySql
{
    public class MySqlFormats : DefaultFormats
    {
        public override string Format => "`{0}`";

        public override string ValueAutoIncrementingQuery => "SELECT LAST_INSERT_ID();";
    }
}