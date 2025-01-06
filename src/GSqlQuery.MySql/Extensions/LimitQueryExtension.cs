using System;

namespace GSqlQuery.MySql
{
    public static class LimitQueryExtension
    {
        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IQueryBuilderWithWhere<SelectQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, SelectQuery<T>>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IAndOr<T, SelectQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, SelectQuery<T>>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T>, QueryOptions> Limit<T>(this IOrderByQueryBuilder<T, OrderByQuery<T>, QueryOptions> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T>>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IQueryBuilderWithWhere<SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, SelectQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IAndOr<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, SelectQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IQueryBuilderWithWhere<Runner.JoinQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, Runner.JoinQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IAndOr<T, Runner.JoinQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, Runner.JoinQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(this IOrderByQueryBuilder<T, OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, uint start, uint? length = null)
            where T : class
        {
            return queryBuilder == null ? throw new ArgumentNullException(nameof(queryBuilder)) : new LimitQueryBuilder<T, OrderByQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder, start, length);
        }
    }
}