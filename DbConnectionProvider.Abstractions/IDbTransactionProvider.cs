using System.Data;

namespace DbConnectionProvider.Abstractions
{
    public interface IDbTransactionProvider
    {
        public void CommitTransaction();
        public void RollbackTransaction();
    }

    public interface IDbTransactionProvider<T> : IDbTransactionProvider
        where T : IDbTransaction
    {
        public T Provide();        
    }
}
