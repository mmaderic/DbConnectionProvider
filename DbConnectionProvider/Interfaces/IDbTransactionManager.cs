using System;

namespace DbConnectionProvider.Interfaces
{
    public interface IDbTransactionManager
    {
        public void SetTransactionOwner(Guid ownerID);
        public bool TrySetTransactionOwner();
        public bool TrySetTransactionOwner(Guid ownerID);
        public void CommitTransaction();
        public void RollbackTransaction();
        public void CommitTransaction(Guid ownerID);
        public void RollbackTransaction(Guid ownerID);
        public void ConcludeTransaction(bool commit);
        public void ConcludeTransaction(bool commit, Guid ownerID);
    }
}
