
namespace DbConnectionProvider.Abstractions
{
    public interface IDbTransactionManager
    {
        public bool TrySetTransactionOwner();
        public void CommitTransaction();
        public void RollbackTransaction();
        public void ConcludeTransaction(bool commit);
    }
}
