using GSqlQuery.MySql.Test.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GSqlQuery.MySql.Test
{
    public class MySqlDatabaseManagementEventsTest
    {
        private readonly MySqlConnectionOptions _connectionOptions;

        public MySqlDatabaseManagementEventsTest()
        {
            _connectionOptions = new MySqlConnectionOptions(Helper.ConnectionString);
        }

        [Fact]
        public void GetParameter()
        {
            var query = Test1.Select(_connectionOptions).Build();

            Queue<ParameterDetail> parameters = new Queue<ParameterDetail>();
            if (query.Criteria != null)
            {
                foreach (var item in query.Criteria.Where(x => x.ParameterDetails != null))
                {
                    foreach (var item2 in item.ParameterDetails)
                    {
                        parameters.Enqueue(item2);
                    }
                }
            }

            var result = _connectionOptions.DatabaseManagement.Events.GetParameter<Test1>(parameters);
            Assert.NotNull(result);
            Assert.Equal(parameters.Count, result.Count());
        }

        [Fact]
        public void OnGetParameter()
        {
            var query = Test1.Select(_connectionOptions).Build();

            Queue<ParameterDetail> parameters = new Queue<ParameterDetail>();
            if (query.Criteria != null)
            {
                foreach (var item in query.Criteria.Where(x => x.ParameterDetails != null))
                {
                    foreach (var item2 in item.ParameterDetails)
                    {
                        parameters.Enqueue(item2);
                    }
                }
            }

            var result = _connectionOptions.DatabaseManagement.Events.OnGetParameter(typeof(Test1), parameters);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}