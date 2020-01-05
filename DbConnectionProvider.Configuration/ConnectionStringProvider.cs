using DbConnectionProvider.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider.Configuration
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IEnumerable<ConnectionStringConfiguration> _connectionStrings;

        public ConnectionStringProvider(IEnumerable<ConnectionStringConfiguration> connectionStrings)
            => _connectionStrings = connectionStrings;

        public string ProvideFor(string identifier)
            => _connectionStrings.Single(x => x.Identifier == identifier).ConnectionString;
    }
}
