using GSqlQuery.MySql.Test.Data.Table;
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
            _connectionOptions = new MySqlConnectionOptions(Helper.GetConnectionString(), new MySqlDatabaseManagementEventsCustom());
        }

        [Fact]
        public void GetParameter()
        {
            var query = Address.Select(_connectionOptions).Build();

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

            var result = _connectionOptions.DatabaseManagement.Events.GetParameter<Address>(parameters);
            Assert.NotNull(result);
            Assert.Equal(parameters.Count, result.Count());
        }

        [Fact]
        public void OnGetParameter()
        {
            var query = Film.Select(_connectionOptions).Build();

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

            var result = _connectionOptions.DatabaseManagement.Events.GetParameter<Film>(parameters);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}