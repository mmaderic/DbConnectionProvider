DbConnectionProvider
=====================

Provides abstracted database connection or transaction objects, which can easily be shared, managed and injected in asp.net core applications.

### Installing DbConnectionProvider

You should install [DbConnectionProvider with NuGet](https://www.nuget.org/packages/DbConnectionProvider):

    Install-Package DbConnectionProvider
    
Or via the .NET Core command line interface:

    dotnet add package DbConnectionProvider

Either commands, from Package Manager Console or .NET Core CLI, will download and install DbConnectionProvider and all required dependencies.


### Basic provider registration example

Code snippet below is using connection provider for Microsoft.Data.SqlClient.SqlConnection and Microsoft.Data.SqlClient.SqlTransaction

    public static void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionStrings = configuration.ReadConnectionStrings();
        serviceCollection.AddSingleton<IDbConnectionStringProvider, ConnectionStringProvider>(
            x => new ConnectionStringProvider(connectionStrings));
        
        services.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>, ConnectionProvider<SqlConnection, SqlTransaction>>(x => 
        {
            var connectionString = x.GetRequiredService<IDbConnectionStringProvider>()
                .ProvideFor(dbConfig.DatabaseName);

             return new ConnectionProvider<SqlConnection, SqlTransaction>(connectionString);
        });
    }


### Transaction manager configuration

In order to use TransactionManager please note it currently does not support distributed transactions. Transactions to multiple databases  will be commited independently. Feature will be added after .NET 5.0 release, when the required support is implemented.

    services.AddScoped<IDbTransactionManager, TransactionManager>();
    services.AddScoped<IDbConnectionProvider>(x => x.GetRequiredService<IDbConnectionProvider<SqlConnection, SqlTransaction>>());

### Multiple providers use case registration example

When you need to provide connection to multiple data sources, using providers with equal type signatures, each should have string identifier provided, in order to be distinct when injected into different contexts. To resolve provider by identifier, use IDbConnectionResolver.

    services.AddScoped<IDbConnectionResolver, ConnectionResolver>();
    services.AddScoped<IDbConnectionProvider<SqlConnection, SqlTransaction>, ConnectionProvider<SqlConnection, SqlTransaction>>(x => 
    {
        var connectionString = x.GetRequiredService<IDbConnectionStringProvider>()
            .ProvideFor(dbConfig.DatabaseName);

        return new ConnectionProvider<SqlConnection, SqlTransaction>(connectionString, dbConfig.DatabaseName);
    });
    
Factory required for transaction manager should include identifier too.
    
    services.AddScoped<IDbTransactionManager, TransactionManager>();
    services.AddScoped<IDbConnectionProvider>(x => x.GetServices<IDbConnectionProvider<SqlConnection, SqlTransaction>>()
        .Single(x => x.Identifier == dbConfig.DatabaseName));


