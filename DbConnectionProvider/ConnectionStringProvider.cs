using DbConnectionProvider.Abstractions;
using DbConnectionProvider.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider
{
    /// <summary>
    /// Connection string provider which distincts and returns connection string defined by string identifier   
    /// It is recomended to register it as a singleton service, using factory with parsed collection of connection string configuration objects passed as a parameter.
    /// </summary>
    public class ConnectionStringProvider : IDbConnectionStringProvider
    {
        private readonly IEnumerable<ConnectionStringConfiguration> _connectionStrings;

        public ConnectionStringProvider(IEnumerable<ConnectionStringConfiguration> connectionStrings)
            => _connectionStrings = connectionStrings;

        public string ProvideFor(string identifier)
            => _connectionStrings.Single(x => x.Identifier == identifier).ConnectionString;

        public int DataSourceCount()
            => _connectionStrings.Count();
    }
}
