using System.Collections.Generic;

namespace GSqlQuery.MySql.BulkCopy
{
    internal class FileBulkLoader
    {
        public string TableName { get; set; }

        public string Path { get; set; }

        public List<string> Columns { get; set; }

        public List<string> Expressions { get; set; }

        public FileBulkLoader(string tableName, string path, List<string> columns, List<string> expressions)
        {
            TableName = tableName;
            Path = path;
            Columns = columns;
            Expressions = expressions;
        }
    }
}