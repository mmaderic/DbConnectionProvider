using DbConnectionProvider.Abstractions;
using DbConnectionProvider.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider
{
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
