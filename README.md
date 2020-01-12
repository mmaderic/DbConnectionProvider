DbConnectionProvider
=====================

Provides abstracted database connection or transaction objects, which can easily be shared, managed and injected in asp.net core applications.

### Installing DbConnectionProvider

You should install [DbConnectionProvider with NuGet](https://www.nuget.org/packages/DbConnectionProvider):

    Install-Package DbConnectionProvider
    
Or via the .NET Core command line interface:

    dotnet add package DbConnectionProvider

Either commands, from Package Manager Console or .NET Core CLI, will download and install DbConnectionProvider and all required dependencies.


### Provider registration example

Code snippet below is using connection provider for Microsoft.Data.SqlClient.SqlConnection

    public static void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionStrings = configuration.ReadConnectionStrings();
        serviceCollection.AddSingleton<IDbConnectionStringProvider, ConnectionStringProvider>(
            x => new ConnectionStringProvider(connectionStrings));
        
        serviceCollection.AddScoped<IDbConnectionProvider<SqlConnection>, ConnectionProvider<SqlConnection>>(x => 
        {
            var connectionString = x.GetRequiredService<IConnectionStringProvider>()
                .ProvideFor(Configuration.Plugins.Identity.DatabaseName);

            return new ConnectionProvider<SqlConnection>(connectionString);
        });

        serviceCollection.AddScoped<IDbTransactionProvider<SqlTransaction>, TransactionProvider<SqlConnection, SqlTransaction>>();
    }


### Transaction manager configuration

In order to use TransactionManager please note it currently does not support distributed transactions. Transactions to multiple databases  will be commited independently. Feature will be added after .NET 5.0 release, when the required support is implemented.

    serviceCollection.AddScoped<IDbTransactionProvider>(x => x.GetRequiredService<IDbTransactionProvider<SqlTransaction>>());





