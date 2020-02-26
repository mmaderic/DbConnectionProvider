using DbConnectionProvider.Abstractions;
using System;
using System.Data;

namespace DbConnectionProvider
{
    /// <summary>
    /// Database connection provider which can be identified by string in case of multiple data source use cases.
    /// Enlists new database connection and transaction objects and provides simple management actions.
    /// </summary>
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
        {
            if (connectionString is null)
                throw new ArgumentException("Connection string must not be null.");

            _connectionString = connectionString;
        } 

        public ConnectionProvider(string connectionString, string identifier) : this(connectionString)
            => Identifier = identifier;

        public TConnection OpenConnection()
        {
            lock (lockObject)
            {
                if (!(_connection is null))
                    throw new InvalidOperationException("Connection has already been enlisted.");

                _connection = new TConnection { ConnectionString = _connectionString };
                _connection.Open();

                return _connection;
            }
        }

        public TConnection GetCurrentConnection()
            =>  _connection;        

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

        public TTransaction BeginTransaction()
        {
            lock (lockObject)
            {
                if (!(_transaction is null))
                    throw new InvalidOperationException("Transaction has already been enlisted.");

                if (_connection is null)
                    throw new InvalidOperationException("There is no enlisted database connection.");

                _transaction = (TTransaction)_connection.BeginTransaction();

                return _transaction;
            }
        }

        public TTransaction GetCurrentTransaction()
            => _transaction;

        public TTransaction ProvideTransaction()
        {
            lock (lockObject)
            {
                if(!(_transaction is null))
                    return _transaction;

                if (_connection is null)
                    throw new InvalidOperationException("There is no enlisted database connection.");
             
                _transaction = (TTransaction) _connection.BeginTransaction();     

                return _transaction;
            }
        }

        public (TConnection connection, TTransaction transaction) ProvideTransactionScoped()
        {
            var connection = ProvideConnection();
            var transaction = ProvideTransaction();

            return (connection, transaction);
        }

        public (TConnection connection, TTransaction transaction) ProvideTransactionLess()
        {
            var connection = ProvideConnection();
            var transaction = GetCurrentTransaction();

            return (connection, transaction);
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
