using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.MySql
{
    public class LimitQuery<T> : Query<T> where T : class
    {
        internal LimitQuery(string text, IEnumerable<PropertyOptions> columns, IEnumerable<CriteriaDetail> criteria, IFormats formats) :
            base(ref text, columns, criteria, formats)
        {
        }
    }

    public class LimitQuery<T, TDbConnection> : LimitQuery<T>, IExecute<IEnumerable<T>, TDbConnection>
        where T : class
    {
        public IDatabaseManagement<TDbConnection> DatabaseManagement { get; }
        private readonly IEnumerable<IDataParameter> _parameters;

        internal LimitQuery(string text, IEnumerable<PropertyOptions> columns, IEnumerable<CriteriaDetail> criteria, ConnectionOptions<TDbConnection> connectionOptions)
            : base(text, columns, criteria, connectionOptions.Formats)
        {
            DatabaseManagement = connectionOptions.DatabaseManagement;
            _parameters = Runner.Extensions.GeneralExtension.GetParameters<T, TDbConnection>(this, DatabaseManagement);
        }

        public IEnumerable<T> Execute()
        {
            return DatabaseManagement.ExecuteReader(this, Columns, _parameters);
        }

        public IEnumerable<T> Execute(TDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }
            return DatabaseManagement.ExecuteReader(dbConnection, this, Columns, _parameters);
        }

        public Task<IEnumerable<T>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return DatabaseManagement.ExecuteReaderAsync(this, Columns, _parameters, cancellationToken);
        }

        public Task<IEnumerable<T>> ExecuteAsync(TDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }
            cancellationToken.ThrowIfCancellationRequested();
            return DatabaseManagement.ExecuteReaderAsync(dbConnection, this, Columns, _parameters, cancellationToken);
        }
    }
}