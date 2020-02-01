using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider.Configurations
{
    public class ConnectionStringConfiguration
    {
        public string Identifier { get; set; }
        public string ConnectionString { get; set; }

        public ConnectionStringConfiguration(string identifier, string connectionString)
        {
            Identifier = identifier;
            ConnectionString = connectionString;
        }
    }

    public static class ConfigurationExtensions
    {
        public static IEnumerable<ConnectionStringConfiguration> ReadConnectionStrings(this IConfiguration configuration, string sectionName = "ConnectionStrings") =>
            configuration.GetSection(sectionName).GetChildren().Select(x => new ConnectionStringConfiguration(x.Key, x.Value));
    }    
}
