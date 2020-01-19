using DbConnectionProvider.Interfaces;
using System;
using System.Data;

namespace DbConnectionProvider
{
    public class ConnectionProvider<TConnection, TTransaction> : IDbConnectionProvider<TConnection, TTransaction>, IDisposable
        where TConnection : IDbConnection, new()
        where TTransaction : IDbTransaction
    {
        private readonly object lockObject = new object();
        private readonly string _connectionString;      
        private TConnection _connection;
        private TTransaction _transaction;

        public string Identifier { get; }

        public ConnectionProvider(string connectionString) 
            => _connectionString = connectionString;

        public ConnectionProvider(string connectionString, string identifier) : this(connectionString)
            => Identifier = identifier;

        public TConnection ProvideConnection()
        {
            lock (lockObject)
            {
                if (_connection is null)
                {
                    _connection = new TConnection { ConnectionString = _connectionString };
                    _connection.Open();
                }

                return _connection;
            }
        }

        public TTransaction ProvideTransaction()
        {
            lock (lockObject)
            {
                if(_transaction is null)                
                    _transaction = (TTransaction)_connection.BeginTransaction();     

                return _transaction;
            }
        }

        public void CommitTransaction()
        {
            lock (lockObject)
            {
                _transaction?.Commit();
                DisposeTransaction();
            }
        }

        public void RollbackTransaction()
        {
            lock (lockObject)
            {
                _transaction?.Rollback();
                DisposeTransaction();
            }
        }

        public void CloseConnection() => Dispose();

        public void DisposeTransaction()
        {
            _transaction?.Dispose();

            _transaction = default;
        }

        public void Dispose()
        {
            lock (lockObject)
            {
                _transaction?.Rollback();
                _transaction?.Dispose();

                _transaction = default;

                _connection?.Close();
                _connection?.Dispose();

                _connection = default;
            }
        }
    }
}
