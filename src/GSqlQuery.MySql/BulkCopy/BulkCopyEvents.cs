using System;

namespace GSqlQuery.MySql.BulkCopy
{
    public class BulkCopyEvents
    {
        private uint _boolCount = 1;

        public virtual ColumnAndExpression GetColumnaAndExpression(PropertyOptions property)
        {
            if (property.PropertyInfo.PropertyType == typeof(bool) || property.PropertyInfo.PropertyType == typeof(bool?))
            {
                string boolName = "@var" + _boolCount++;
                string expresion = "{0} = ({1} = 'True');".Replace("{0}", property.ColumnAttribute.Name).Replace("{1}", boolName);
                return new ColumnAndExpression(boolName, expresion);
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
