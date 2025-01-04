using GSqlQuery.Cache;
using System.Collections;
using System.Collections.Generic;

namespace GSqlQuery.MySql
{
    internal abstract class LimitQueryBuilderBase<T, TReturn, TQueryOptions, TQuery>(IBuilder<TQuery> queryBuilder, IQueryOptions<TQueryOptions> queryOptions, uint start, uint? length) : QueryBuilderBase<T, TReturn, TQueryOptions>(queryOptions.QueryOptions)
        where T : class
        where TReturn : IQuery<T, TQueryOptions>
        where TQuery : IQuery<T, TQueryOptions>
        where TQueryOptions : QueryOptions
    {
        protected readonly TQuery _query = queryBuilder.Build();
        protected readonly uint _start = start;
        protected readonly uint? _length = length;

        internal string CreateQueryText()
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

        public override TReturn Build()
        {
            string text = CreateQueryText();
            TReturn result = GetQuery(text, _query.Columns, _query.Criteria, QueryOptions);
            return result;
        }

        public abstract TReturn GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, TQueryOptions queryOptions);
    }

    internal class LimitQueryBuilder<T, TQuery>(IBuilder<TQuery> queryBuilder, IQueryOptions<QueryOptions> queryOptions, uint start, uint? length) : LimitQueryBuilderBase<T, LimitQuery<T>, QueryOptions, TQuery>(queryBuilder, queryOptions, start, length)
        where T : class
        where TQuery : IQuery<T, QueryOptions>
    {
        public override LimitQuery<T> GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, QueryOptions queryOptions)
        {
            return new LimitQuery<T>(text, _classOptions.FormatTableName.Table, columns, criteria, queryOptions);
        }
    }

    internal class LimitQueryBuilder<T, TQuery, TDbConnection>(IBuilder<TQuery> queryBuilder, IQueryOptions<ConnectionOptions<TDbConnection>> queryOptions, uint start, uint? length) :  LimitQueryBuilderBase<T, LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TQuery>(queryBuilder, queryOptions, start, length),
        IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
        where TQuery : IQuery<T, ConnectionOptions<TDbConnection>>
    {
        public override LimitQuery<T, TDbConnection> GetQuery(string text, PropertyOptionsCollection columns, IEnumerable<CriteriaDetailCollection> criteria, ConnectionOptions<TDbConnection> queryOptions)
        {
            return new LimitQuery<T, TDbConnection>(text, _classOptions.FormatTableName.Table, columns, criteria, queryOptions);
        }
    }
}