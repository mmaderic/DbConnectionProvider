DbConnectionProvider
=====================

Provides abstracted database connection or transaction objects, which can easily be shared and injected in asp.net core applications.

### Installing DbConnectionProvider

You should install [DbConnectionProvider with NuGet](https://www.nuget.org/packages/DbConnectionProvider):

    Install-Package DbConnectionProvider
    
Or via the .NET Core command line interface:

    dotnet add package DbConnectionProvider

Either commands, from Package Manager Console or .NET Core CLI, will download and install DbConnectionProvider and all required dependencies.


### Provider registration example

Code snipped below is using connection provider for Microsoft.Data.SqlClient.SqlConnection

    services.AddScoped<IDbConnectionProvider<SqlConnection>, ConnectionProvider<SqlConnection>>(x => 
    {
        var connectionString = x.GetRequiredService<IConnectionStringProvider>()
            .ProvideFor(Configuration.Plugins.Identity.DatabaseName);

        return new ConnectionProvider<SqlConnection>(connectionString);
    });

    services.AddScoped<IDbTransactionProvider<SqlTransaction>, TransactionProvider<SqlConnection, SqlTransaction>>();




