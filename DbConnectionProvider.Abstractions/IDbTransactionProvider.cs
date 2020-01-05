using System.Data;

namespace DbConnectionProvider.Abstractions
{
    public interface IDbTransactionProvider<T>
        where T : IDbTransaction
    {
        public T Provide();
        public void CommitTransaction();
        public void RollbackTransaction();
    }
}
