using DbConnectionProvider.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DbConnectionProvider.SqlServer
{
    public static class ServiceCollectionExtensions
    {   
        public static IServiceCollection AddSqlConnectionProvider(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionStrings = configuration.ReadConnectionStrings();

            serviceCollection.AddSingleton<IDbConnectionStringProvider, ConnectionStringProvider>(
               x => new ConnectionStringProvider(connectionStrings));

            foreach (var connection in connectionStrings)
            {
                serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                    x => new SqlConnectionProvider(connection.ConnectionString, connection.Identifier));

                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
                    .Single(x => x.Identifier == connection.Identifier));
            }

            serviceCollection.AddScoped<IDbConnectionResolver, ConnectionResolver>();
            serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();

            return serviceCollection;
        }
    }
}
