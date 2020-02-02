using DbConnectionProvider.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbConnectionProvider
{
    public class ConnectionResolver : IDbConnectionResolver
    {
        /// <summary>
        /// Database connection resolver which distincts and returns connection provider defined by string identifier.
        /// It is recomended to register it as a scoped service.
        /// Please note you need to register IDbConnectionProvider base interfaces using factory method. Refer to github documentation for example.
        /// </summary>
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
