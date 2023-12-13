using System;

namespace GSqlQuery.MySql.BulkCopy
{
    public struct ColumnAndExpression
    {
        public string ColumnName { get; set; }

        public string Expression { get; set; }

        public ColumnAndExpression(string column)
        {
            ColumnName = column ?? throw new ArgumentNullException(nameof(column));
        }

        public ColumnAndExpression(string column, string expression) : this(column)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
    }
}
