using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider.Configuration
{
    public static class IConfigurationExtensions
    {
        public static IEnumerable<ConnectionStringConfiguration> ReadConnectionStrings(this IConfiguration configuration, string sectionName = "ConnectionStrings") =>
            configuration.GetSection(sectionName).GetChildren().Select(x => new ConnectionStringConfiguration(x.Key, x.Value));
    }
}
