using System;

namespace DbConnectionProvider.Abstractions
{
    /// <summary>
    /// Defines database transaction manager which triggers actions on enlisted transaction objects.
    /// Actions can be locked by owner identifier.
    /// Please note it currently does not support distributed transactions. Transactions to multiple databases will be commited independently.
    /// Feature will be added after .NET 5.0 release, when the required support is implemented.
    /// </summary>
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
