using GSqlQuery.MySql.BulkCopy;
using System;
using Xunit;

namespace GSqlQuery.MySql.Test.BulkCopy
{
    public class ColumnAndExpressionTest
    {
        [Fact]
        public void Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ColumnAndExpression(null));
            Assert.Throws<ArgumentNullException>(() => new ColumnAndExpression(null, null));
            Assert.Throws<ArgumentNullException>(() => new ColumnAndExpression("string", null));
        }
    }
}
