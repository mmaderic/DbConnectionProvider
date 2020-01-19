using DbConnectionProvider.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DbConnectionProvider
{
    public class TransactionManager : IDbTransactionManager
    {
        private readonly IEnumerable<IDbConnectionProvider> _connectionProviders;
        private Guid? _ownerID;       

        public TransactionManager(IServiceProvider servicesProvider) =>
            _connectionProviders = servicesProvider.GetServices<IDbConnectionProvider>();

        public void SetTransactionOwner(Guid ownerID)
        {
            if (!(_ownerID is null))
                throw new InvalidOperationException("Transaction owner is already set.");

            _ownerID = ownerID;
        }

        public bool TrySetTransactionOwner()
        {
            if (!(_ownerID is null))
                return false;

            _ownerID = Guid.Empty;
            return true;
        }

        public bool TrySetTransactionOwner(Guid ownerID)
        {
            if (!(_ownerID is null))
                return false;

            _ownerID = ownerID;
            return true;
        }

        public void CommitTransaction(Guid ownerID)
        {
            if (ownerID != _ownerID)
                throw new InvalidOperationException("Wrong transaction owner ID provided.");

            foreach (var provider in _connectionProviders)
                provider.CommitTransaction();
        }

        public void RollbackTransaction(Guid ownerID)
        {
            if (ownerID != _ownerID)
                throw new InvalidOperationException("Wrong transaction owner ID provided.");

            foreach (var provider in _connectionProviders)
                provider.RollbackTransaction();
        }

        public void CommitTransaction()
        {
            if (_ownerID != Guid.Empty)
                throw new InvalidOperationException("Transaction is locked with ownerID");

            foreach (var provider in _connectionProviders)
                provider.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            if (_ownerID != Guid.Empty)
                throw new InvalidOperationException("Transaction is locked with ownerID");

            foreach (var provider in _connectionProviders)
                provider.RollbackTransaction();
        }

        public void ConcludeTransaction(bool commit, Guid ownerID)
        {
            if (commit)
                CommitTransaction(ownerID);
            else RollbackTransaction(ownerID);

            _ownerID = null;
        }

        public void ConcludeTransaction(bool commit)
        {
            if (commit)
                CommitTransaction();
            else RollbackTransaction();

            _ownerID = null;
        }
    }
}
