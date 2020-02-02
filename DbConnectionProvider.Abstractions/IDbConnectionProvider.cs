using System.Data;

namespace DbConnectionProvider.Abstractions
{
    /// <summary>
    /// Defines a base database connection provider which can be identified by string in case of multiple data source use cases.
    /// Provides simple management actions against enlisted connection and transaction objects.
    /// </summary>
    public interface IDbConnectionProvider
    {
        public string Identifier { get; }
        public void CloseConnection();
        public void CommitTransaction();
        public void RollbackTransaction();
    }

    /// <summary>
    /// Defines a generic database connection provider used for enlistment of connection and transaction objects
    /// </summary>
    public interface IDbConnectionProvider<TConnection, TTransaction> : IDbConnectionProvider
    where TConnection : IDbConnection
    where TTransaction : IDbTransaction
    {
        /// <summary>
        /// Tries to open new database connection. Throws exception if connection has been already enlisted.
        /// </summary>
        /// <returns>New database connection object</returns>
        public TConnection OpenConnection();

        /// <summary>
        /// Returning already enlisted connection object.
        /// </summary>
        /// <returns>Enlisted or null database connection object</returns>
        public TConnection GetCurrentConnection();

        /// <summary>
        /// Will provide new or return already enlisted connection object
        /// </summary>
        /// <returns>Enlisted or new database connection object</returns>
        public TConnection ProvideConnection();

        /// <summary>
        /// Tries to open begin new database transaction. Throws exception if transaction has already been enlisted.
        /// </summary>
        /// <returns>New database transaction object</returns>
        public TTransaction BeginTransaction();

        /// <summary>
        /// Returning already enlisted transaction object.
        /// </summary>
        /// <returns>Enlisted or null database transaction object</returns>
        public TTransaction GetCurrentTransaction();

        /// <summary>
        /// Will provide new or return already enlisted transaction object
        /// </summary>
        /// <returns>Enlisted or new database transaction object</returns>
        public TTransaction ProvideTransaction();

        /// <summary>
        /// Will provide connection with enlisted transaction objects tuple
        /// </summary>
        /// <returns>Enlisted or new database connection and transaction objects tuple</returns>
        public (TConnection connection, TTransaction transaction) ProvideTransactionScoped();
    }
}
