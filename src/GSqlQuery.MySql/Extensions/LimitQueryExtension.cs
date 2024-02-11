using System;

namespace GSqlQuery.MySql
{
    public static class LimitQueryExtension
    {
        public static IQueryBuilder<LimitQuery<T>, IFormats> Limit<T>(this IQueryBuilderWithWhere<SelectQuery<T>, IFormats> queryBuilder, int start, int? length)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, SelectQuery<T>>(queryBuilder, queryBuilder.Options, start, length);
        }

        public static IQueryBuilder<LimitQuery<T>, IFormats> Limit<T>(this IAndOr<T, SelectQuery<T>> queryBuilder, int start, int? length) 
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, SelectQuery<T>>(queryBuilder, queryBuilder.Build().Formats, start, length);
        }

        public static IQueryBuilder<LimitQuery<T>, IFormats> Limit<T>(this IQueryBuilder<OrderByQuery<T>, IFormats> queryBuilder, int start, int? length) 
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }
            return new LimitQueryBuilder<T, OrderByQuery<T>>(queryBuilder, queryBuilder.Options, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(
            this IQueryBuilderWithWhere<SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length) 
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, SelectQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder.Options, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(
            this IAndOr<T, SelectQuery<T, TDbConnection>> queryBuilder, int start, int? length) 
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }
            SelectQuery<T, TDbConnection> query = queryBuilder.Build();
            return new LimitQueryBuilder<T, SelectQuery<T, TDbConnection>, TDbConnection>(queryBuilder,
                new ConnectionOptions<TDbConnection>(query.Formats, query.DatabaseManagement), start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(
            this IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length) where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, OrderByQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder.Options, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(
            this IQueryBuilderWithWhere<JoinQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, int start, int? length) 
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return new LimitQueryBuilder<T, JoinQuery<T, TDbConnection>, TDbConnection>(queryBuilder, queryBuilder.Options, start, length);
        }

        public static IQueryBuilder<LimitQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Limit<T, TDbConnection>(
            this IAndOr<T, JoinQuery<T, TDbConnection>> queryBuilder, int start, int? length)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }
            JoinQuery<T, TDbConnection> query = queryBuilder.Build();

            return new LimitQueryBuilder<T, JoinQuery<T, TDbConnection>, TDbConnection>(queryBuilder,
                new ConnectionOptions<TDbConnection>(query.Formats, query.DatabaseManagement), start, length);
        }
    }
}