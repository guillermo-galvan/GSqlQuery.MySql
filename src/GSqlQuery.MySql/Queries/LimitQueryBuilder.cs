using System.Linq;

namespace GSqlQuery.MySql
{
    internal abstract class LimitQueryBuilderBase<T, TReturn, TOptions, TSelectQuery, TOrderQuery> : QueryBuilderBase<T, TReturn>
        where T : class, new()
        where TReturn : LimitQuery<T>
        where TSelectQuery : SelectQuery<T>
        where TOrderQuery : OrderByQuery<T>
    {
        protected readonly IQuery<T> _selectQuery;
        protected readonly int _start;
        protected readonly int? _length;

        public LimitQueryBuilderBase(IQueryBuilderWithWhere<TSelectQuery, TOptions> queryBuilder, IStatements statements, int start, int? length)
            : base(statements)
        {
            _selectQuery = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        public LimitQueryBuilderBase(IAndOr<T, TSelectQuery> queryBuilder, IStatements statements, int start, int? length)
            : base(statements)
        {
            _selectQuery = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        public LimitQueryBuilderBase(IQueryBuilder<TOrderQuery, TOptions> queryBuilder, IStatements statements, int start, int? length)
           : base(statements)
        {
            _selectQuery = queryBuilder.Build();
            _start = start;
            _length = length;
            Columns = Enumerable.Empty<PropertyOptions>();
        }

        internal string GenerateQuery()
        {
            string result = _selectQuery.Text.Replace(";", "");
            result = _length.HasValue ? $"{result} LIMIT {_start},{_length};" : $"{result} LIMIT {_start};";
            return result;
        }

    }

    internal class LimitQueryBuilder<T> : LimitQueryBuilderBase<T, LimitQuery<T>, IStatements, SelectQuery<T>, OrderByQuery<T>>
        where T : class, new()
    {
        public LimitQueryBuilder(IQueryBuilderWithWhere<SelectQuery<T>, IStatements> queryBuilder, IStatements statements, int start, int? length)
            : base(queryBuilder, statements, start, length)
        { }

        public LimitQueryBuilder(IAndOr<T, SelectQuery<T>> queryBuilder, IStatements statements, int start, int? length)
            : base(queryBuilder, statements, start, length)
        { }

        public LimitQueryBuilder(IQueryBuilder<OrderByQuery<T>, IStatements> queryBuilder, IStatements statements, int start, int? length)
            : base(queryBuilder, statements, start, length)
        { }

        public override LimitQuery<T> Build()
        {
            var query = GenerateQuery();
            return new LimitQuery<T>(query, _selectQuery.Columns, _selectQuery.Criteria, Options);
        }


    }

    internal class LimitQueryBuilder<T, TDbConnection> :
        LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, SelectQuery<T, TDbConnection>, OrderByQuery<T, TDbConnection>>,
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class, new()
    {
        new public ConnectionOptions<TDbConnection> Options { get; }

        public LimitQueryBuilder(IQueryBuilderWithWhere<SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Statements, start, length)
        {
            Options = connectionOptions;
        }

        public LimitQueryBuilder(IAndOr<T, SelectQuery<T, TDbConnection>> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Statements, start, length)
        {
            Options = connectionOptions;
        }

        public LimitQueryBuilder(IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder,
            ConnectionOptions<TDbConnection> connectionOptions, int start, int? length)
            : base(queryBuilder, connectionOptions.Statements, start, length)
        {
            Options = connectionOptions;
        }

        public override LimitQuery<T, TDbConnection> Build()
        {
            var query = GenerateQuery();
            return new LimitQuery<T, TDbConnection>(query, _selectQuery.Columns, _selectQuery.Criteria, Options);
        }
    }
}