using DbConnectionProvider.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DbConnectionProvider
{
    public class TransactionManager : IDbTransactionManager
    {
        private readonly IEnumerable<IDbTransactionProvider<IDbTransaction>> _transactionProviders;
        private bool _transactionOwnerSet;
        private bool _rollBackTransaction;

        public TransactionManager(IServiceProvider serviceProvider) =>
            _transactionProviders = serviceProvider.GetServices<IDbTransactionProvider<IDbTransaction>>();

        public bool TrySetTransactionOwner()
        {
            if (_transactionOwnerSet)
                return false;

            _transactionOwnerSet = true;
            return true;
        }

        public void CommitTransaction()
        {
            if (_rollBackTransaction)
                return;

            Parallel.ForEach(_transactionProviders,
                provider => provider.CommitTransaction());
        }

        public void RollbackTransaction()
        {
            if (_rollBackTransaction)
                return;

            Parallel.ForEach(_transactionProviders,
                provider => provider.RollbackTransaction());

            _rollBackTransaction = true;
        }

        public void ConcludeTransaction(bool commit)
        {
            if (commit)
                CommitTransaction();
            else RollbackTransaction();

            _transactionOwnerSet = false;
        }
    }
}
