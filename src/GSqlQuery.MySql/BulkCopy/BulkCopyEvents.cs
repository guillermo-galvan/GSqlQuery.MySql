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

    public class BulkCopyEvents
    {
        private uint _boolCount = 1;

        public virtual ColumnAndExpression GetColumnaAndExpression(PropertyOptions property)
        {
            if (property.PropertyInfo.PropertyType == typeof(bool) || property.PropertyInfo.PropertyType == typeof(bool?))
            {
                var boolName = $"@var{_boolCount++}";

                return new ColumnAndExpression(boolName, $"{property.ColumnAttribute.Name} = ({boolName} = 'True');");
            }
            else
            {
                return new ColumnAndExpression(property.ColumnAttribute.Name);
            }
        }

        public virtual object Format(PropertyOptions property, object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (property.PropertyInfo.PropertyType == typeof(DateTime) || property.PropertyInfo.PropertyType == typeof(DateTime?))
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (property.PropertyInfo.PropertyType == typeof(bool) || property.PropertyInfo.PropertyType == typeof(bool?))
            {
                return (bool)value ? "True" : "False";
            }
            else
            {
                return value;
            }
        }
    }
}
