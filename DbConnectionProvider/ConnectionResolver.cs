using DbConnectionProvider.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbConnectionProvider
{
    public class ConnectionResolver : IDbConnectionResolver
    {
        private readonly IEnumerable<IDbConnectionProvider> _connectionProviders;

        public ConnectionResolver(IServiceProvider servicesProvider) =>
            _connectionProviders = servicesProvider.GetServices<IDbConnectionProvider>();

        public IDbConnectionProvider ResolveFor(string identifier)
            => _connectionProviders.Single(x => x.Identifier == identifier);

        public IDbConnectionProvider<TConnection, TTransaction> ResolveFor<TConnection, TTransaction>(string identifier)
            where TConnection : IDbConnection
            where TTransaction : IDbTransaction
            => (IDbConnectionProvider<TConnection, TTransaction>) _connectionProviders.Single(x => x.Identifier == identifier);

        public void CloseConnectionFor(string identifier)
            => _connectionProviders.Single(x => x.Identifier == identifier).CloseConnection();

        public void CloseAllConnections()
        {
            foreach (var provider in _connectionProviders)
                provider.CloseConnection();
        }        
    }
}
