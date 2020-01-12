using DbConnectionProvider.Abstractions;
using System;
using System.Data;

namespace DbConnectionProvider
{
    public class ConnectionProvider<TConnection> : IDbConnectionProvider<TConnection>, IDisposable
        where TConnection : IDbConnection, new()
    {
        private readonly object lockObject = new object();
        private readonly string _connectionString;
        private TConnection _connection;

        public ConnectionProvider(string connectionString) => _connectionString = connectionString;

        public TConnection Provide()
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

        public void Close() => Dispose();

        public void Dispose()
        {
            lock (lockObject)
            {
                _connection?.Close();
                _connection?.Dispose();

                _connection = default;
            }
        }
    }
}
