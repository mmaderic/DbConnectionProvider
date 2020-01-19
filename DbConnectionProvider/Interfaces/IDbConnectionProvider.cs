using System.Data;

namespace DbConnectionProvider.Interfaces
{
    public interface IDbConnectionProvider
    {
        public string Identifier { get; }
        public void CloseConnection();
        public void CommitTransaction();
        public void RollbackTransaction();
    }

    public interface IDbConnectionProvider<TConnection, TTransaction> : IDbConnectionProvider
    where TConnection : IDbConnection
    where TTransaction : IDbTransaction
    {
        public TConnection ProvideConnection();
        public TTransaction ProvideTransaction();
    }
}
