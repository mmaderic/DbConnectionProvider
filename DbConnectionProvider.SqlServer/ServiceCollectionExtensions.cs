using DbConnectionProvider.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbConnectionProvider.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Reads configuration file for available connection strings and registers sql server connection providers. 
        /// Registers connection resolver and transaction manager. Recomended when you have multiple data sources.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProviders(
            this IServiceCollection serviceCollection, IConfiguration configuration, string connectionStringsSection = "ConnectionStrings")
        {
            var connectionStringConfigurations = configuration.ReadConnectionStrings(connectionStringsSection);

            return serviceCollection.AddSqlConnectionProviders(connectionStringConfigurations);
        }

        /// <summary>
        /// Registers connection providers using provided connection string configurations.
        /// Registers connection resolver and transaction manager. Recomended when you have multiple data sources.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProviders(
            this IServiceCollection serviceCollection, IEnumerable<ConnectionStringConfiguration> connectionStringConfigurations)
        {
            serviceCollection.AddSingleton(x => new ConnectionStringProvider(connectionStringConfigurations));
            serviceCollection.AddSingleton<IDbConnectionStringProvider>(x => x.GetRequiredService<ConnectionStringProvider>());

            foreach (var connection in connectionStringConfigurations)
            {
                serviceCollection.AddScoped(x => new SqlConnectionProvider(connection.ConnectionString, connection.Identifier));

                serviceCollection.AddScoped<IDbConnectionProvider>(
                    x => x.GetServices<SqlConnectionProvider>().Single(x => x.Identifier == connection.Identifier));

                serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                    x => x.GetServices<SqlConnectionProvider>().Single(x => x.Identifier == connection.Identifier));
            }

            serviceCollection.AddScoped<ConnectionResolver>();
            serviceCollection.AddScoped<TransactionManager>();

            serviceCollection.AddScoped<IDbConnectionResolver>(x => x.GetRequiredService<ConnectionResolver>());
            serviceCollection.AddScoped<IDbTransactionManager>(x => x.GetRequiredService<TransactionManager>());

            return serviceCollection;
        }

        /// <summary>
        /// Reads configuration file for available connection string and registers sql server connection provider. 
        /// Optionaly registers transaction manager. Recomended when you have single data source.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(
            this IServiceCollection serviceCollection, IConfiguration configuration, bool registerTransactionManager = true, string connectionStringsSection = "ConnectionStrings")
        {
            var connectionStringConfiguration = configuration.ReadConnectionStrings(connectionStringsSection).Single();

            return serviceCollection.AddSqlConnectionProvider(connectionStringConfiguration.ConnectionString, registerTransactionManager);
        }

        /// <summary>  
        /// Register connection resolver and optional transaction manager with provided connection string. 
        /// Recomended when you have single data source.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(this IServiceCollection serviceCollection, string connectionString, bool registerTransactionManager = true)
        {
            serviceCollection.AddScoped(x => new SqlConnectionProvider(connectionString));
            serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(x => x.GetRequiredService<SqlConnectionProvider>());

            if (registerTransactionManager)
            {
                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetRequiredService<SqlConnectionProvider>());

                serviceCollection.AddScoped<TransactionManager>();
                serviceCollection.AddScoped<IDbTransactionManager>(x => x.GetRequiredService<TransactionManager>());
            }

            return serviceCollection;
        }

        /// <summary>  
        /// Register connection provider using implementation factory
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(
            this IServiceCollection serviceCollection, Func<IServiceProvider, SqlConnectionProvider> implementationFactory, bool registerTransactionManager = true)
        {
            serviceCollection.AddScoped(implementationFactory);
            serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(x => x.GetRequiredService<SqlConnectionProvider>());

            if (registerTransactionManager)
            {
                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetRequiredService<SqlConnectionProvider>());

                serviceCollection.AddScoped<TransactionManager>();
                serviceCollection.AddScoped<IDbTransactionManager>(x => x.GetRequiredService<TransactionManager>());
            }

            return serviceCollection;
        } 
    }
}
