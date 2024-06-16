using System.Linq;

namespace GSqlQuery.MySql
{
    internal abstract class LimitQueryBuilderBase<T, TReturn, TQueryOptions, TQuery> : QueryBuilderBase<T, TReturn, TQueryOptions>
        where T : class
        where TReturn : IQuery<T, TQueryOptions>
        where TQuery : IQuery<T, TQueryOptions>
        where TQueryOptions : QueryOptions
    {
        protected readonly TQuery _query;
        protected readonly int _start;
        protected readonly int? _length;

        public LimitQueryBuilderBase(IQueryBuilderWithWhere<TQuery, TQueryOptions> queryBuilder, int start, int? length)
            : base(queryBuilder.QueryOptions)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = [];
        }

        public LimitQueryBuilderBase(IAndOr<T, TQuery, TQueryOptions> queryBuilder, int start, int? length)
            : base(queryBuilder.QueryOptions)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        public LimitQueryBuilderBase(IQueryBuilder<TQuery, TQueryOptions> queryBuilder, int start, int? length)
           : base(queryBuilder.QueryOptions)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        internal string GenerateQuery()
        {
            string result = _query.Text.Replace(";", "");

            if (_length.HasValue)
            {
                return "{0} LIMIT {1},{2};".Replace("{0}", result).Replace("{1}", _start.ToString()).Replace("{2}", _length.ToString());
            }
            else
            {
                return "{0} LIMIT {1};".Replace("{0}", result).Replace("{1}", _start.ToString());
            }
        }

    }

    internal class LimitQueryBuilder<T, TQuery> : LimitQueryBuilderBase<T, LimitQuery<T>, QueryOptions, TQuery>
        where T : class
        where TQuery : IQuery<T, QueryOptions>
    {
        public LimitQueryBuilder(IQueryBuilderWithWhere<TQuery, QueryOptions> queryBuilder, int start, int? length)
            : base(queryBuilder, start, length)
        { }

        public LimitQueryBuilder(IAndOr<T, TQuery, QueryOptions> queryBuilder, int start, int? length)
            : base(queryBuilder, start, length)
        { }

        public LimitQueryBuilder(IQueryBuilder<TQuery, QueryOptions> queryBuilder, int start, int? length)
            : base(queryBuilder, start, length)
        { }

        public override LimitQuery<T> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T>(query, _query.Columns, _query.Criteria, QueryOptions);
        }


    }

    internal class LimitQueryBuilder<T, TQuery, TDbConnection> :  LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TQuery>,
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
        where TQuery : IQuery<T, ConnectionOptions<TDbConnection>>
    {
        public LimitQueryBuilder(IQueryBuilderWithWhere<TQuery, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length)
            : base(queryBuilder, start, length)
        {  }

        public LimitQueryBuilder(IAndOr<T, TQuery, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length)
            : base(queryBuilder, start, length)
        { }

        public LimitQueryBuilder(IQueryBuilder<TQuery, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length) : base(queryBuilder, start, length)
        { }

        public override LimitQuery<T, TDbConnection> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T, TDbConnection>(query, _query.Columns, _query.Criteria, QueryOptions);
        }
    }
}