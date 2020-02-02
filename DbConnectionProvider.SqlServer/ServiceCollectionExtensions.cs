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
            var connectionStrings = configuration.ReadConnectionStrings(connectionStringsSection);

            serviceCollection.AddSingleton<IDbConnectionStringProvider, ConnectionStringProvider>(
               x => new ConnectionStringProvider(connectionStrings));

            foreach (var connection in connectionStrings)
            {
                serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                    x => new SqlConnectionProvider(connection.ConnectionString, connection.Identifier));

                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
                    .Single(x => x.Identifier == connection.Identifier));

                serviceCollection.AddScoped(x => (SqlConnectionProvider) x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
                    .Single(x => x.Identifier == connection.Identifier));
            }

            serviceCollection.AddScoped<IDbConnectionResolver, ConnectionResolver>();
            serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();

            return serviceCollection;
        }

        /// <summary>
        /// Registers connection providers using provided connection string configurations.
        /// Registers connection resolver and transaction manager. Recomended when you have multiple data sources.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProviders(
            this IServiceCollection serviceCollection, IEnumerable<ConnectionStringConfiguration> connectionStringConfigurations)
        {
            serviceCollection.AddSingleton<IDbConnectionStringProvider, ConnectionStringProvider>(
                x => new ConnectionStringProvider(connectionStringConfigurations));

            foreach (var connection in connectionStringConfigurations)
            {
                serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                    x => new SqlConnectionProvider(connection.ConnectionString, connection.Identifier));

                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
                    .Single(x => x.Identifier == connection.Identifier));

                serviceCollection.AddScoped(x => (SqlConnectionProvider)x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
                    .Single(x => x.Identifier == connection.Identifier));
            }

            serviceCollection.AddScoped<IDbConnectionResolver, ConnectionResolver>();
            serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();

            return serviceCollection;
        }

        /// <summary>
        /// Reads configuration file for available connection string and registers sql server connection provider. 
        /// Optionaly registers transaction manager. Recomended when you have single data source.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(
            this IServiceCollection serviceCollection, IConfiguration configuration, bool registerTransactionManager = true, string connectionStringsSection = "ConnectionStrings")
        {
            var connectionString = configuration.ReadConnectionStrings(connectionStringsSection).Single();

            serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                x => new SqlConnectionProvider(connectionString.ConnectionString));

            serviceCollection.AddScoped(x => (SqlConnectionProvider)x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());

            if (registerTransactionManager)
            {
                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());
                serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();
            }

            return serviceCollection;
        }

        /// <summary>  
        /// Register connection resolver and optional transaction manager with provided connection string. 
        /// Recomended when you have single data source.
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(this IServiceCollection serviceCollection, string connectionString, bool registerTransactionManager = true)
        {
            serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(
                x => new SqlConnectionProvider(connectionString));

            serviceCollection.AddScoped(x => (SqlConnectionProvider) x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());

            if (registerTransactionManager)
            {
                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());
                serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();
            }

            return serviceCollection;
        }

        /// <summary>  
        /// Register connection provider using implementation factory
        /// </summary>
        public static IServiceCollection AddSqlConnectionProvider(
            this IServiceCollection serviceCollection, Func<IServiceProvider, SqlConnectionProvider> implementationFactory, bool registerTransactionManager = true)
        {
            serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>>(implementationFactory);
            serviceCollection.AddScoped(x => (SqlConnectionProvider) x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());

            if (registerTransactionManager)
            {
                serviceCollection.AddScoped<IDbConnectionProvider>(x => x.GetService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());
                serviceCollection.AddScoped<IDbTransactionManager, TransactionManager>();
            }

            return serviceCollection;
        }
    }
}
