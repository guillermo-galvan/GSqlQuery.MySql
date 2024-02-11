using System.Linq;

namespace GSqlQuery.MySql
{
    internal abstract class LimitQueryBuilderBase<T, TReturn, TOptions, TQuery> : QueryBuilderBase<T, TReturn>
        where T : class
        where TReturn : LimitQuery<T>
        where TQuery : IQuery<T>
    {
        protected readonly TQuery _query;
        protected readonly int _start;
        protected readonly int? _length;

        public LimitQueryBuilderBase(IQueryBuilderWithWhere<TQuery, TOptions> queryBuilder, IFormats formats, int start, int? length)
            : base(formats)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        public LimitQueryBuilderBase(IAndOr<T, TQuery> queryBuilder, IFormats formats, int start, int? length)
            : base(formats)
        {
            _query = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        public LimitQueryBuilderBase(IQueryBuilder<TQuery, TOptions> queryBuilder, IFormats formats, int start, int? length)
           : base(formats)
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

    internal class LimitQueryBuilder<T, TQuery> : LimitQueryBuilderBase<T, LimitQuery<T>, IFormats, TQuery>
        where T : class
        where TQuery : IQuery<T>
    {
        public LimitQueryBuilder(IQueryBuilderWithWhere<TQuery, IFormats> queryBuilder, IFormats formats, int start, int? length)
            : base(queryBuilder, formats, start, length)
        { }

        public LimitQueryBuilder(IAndOr<T, TQuery> queryBuilder, IFormats formats, int start, int? length)
            : base(queryBuilder, formats, start, length)
        { }

        public LimitQueryBuilder(IQueryBuilder<TQuery, IFormats> queryBuilder, IFormats formats, int start, int? length)
            : base(queryBuilder, formats, start, length)
        { }

        public override LimitQuery<T> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T>(query, _query.Columns, _query.Criteria, Options);
        }


    }

    internal class LimitQueryBuilder<T, TQuery, TDbConnection> :
        LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TQuery>,
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
        where TQuery : IQuery<T>
    {
        new public ConnectionOptions<TDbConnection> Options { get; }

        public LimitQueryBuilder(IQueryBuilderWithWhere<TQuery, ConnectionOptions<TDbConnection>> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Formats, start, length)
        {
            Options = connectionOptions;
        }

        public LimitQueryBuilder(IAndOr<T, TQuery> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Formats, start, length)
        {
            Options = connectionOptions;
        }

        public LimitQueryBuilder(IQueryBuilder<TQuery, ConnectionOptions<TDbConnection>> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Formats, start, length)
        {
            Options = connectionOptions;
        }

        public override LimitQuery<T, TDbConnection> Build()
        {
            string query = GenerateQuery();
            return new LimitQuery<T, TDbConnection>(query, _query.Columns, _query.Criteria, Options);
        }
    }
}