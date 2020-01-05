using DbConnectionProvider.Abstractions;
using System;
using System.Data;

namespace DbConnectionProvider
{
    public class TransactionProvider<TConnection, TTransaction> : IDbTransactionProvider<TTransaction>, IDisposable
        where TConnection : IDbConnection
        where TTransaction : IDbTransaction
    {
        private readonly IDbConnectionProvider<TConnection> _connectionProvider;
        private TTransaction _sqlTransaction;
        private bool _commitTriggered;
        private bool _rollbackTriggered;

        public TransactionProvider(IDbConnectionProvider<TConnection> dbConnectionProvider)
            => _connectionProvider = dbConnectionProvider;

        public TTransaction Provide()
        {
            if (_sqlTransaction is null)
            {
                var connection = _connectionProvider.Provide();
                _sqlTransaction = (TTransaction) connection.BeginTransaction();
            }

            return _sqlTransaction;
        }

        public void CommitTransaction()
        {
            _sqlTransaction?.Commit();
            _commitTriggered = true;
            Dispose();
        }

        public void RollbackTransaction()
        {
            _sqlTransaction?.Rollback();
            _rollbackTriggered = true;
            Dispose();
        }

        public void Dispose()
        {
            if (!_commitTriggered && !_rollbackTriggered)
                _sqlTransaction?.Rollback();

            _sqlTransaction?.Dispose();

            _sqlTransaction = default;
            _commitTriggered = false;
            _rollbackTriggered = false;
        }
    }
}
