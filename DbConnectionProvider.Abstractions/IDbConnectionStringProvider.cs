
namespace DbConnectionProvider.Abstractions
{
    /// <summary>
    /// Defines database connection string provider which distincts and returns connection string defined by string identifier   
    /// </summary>
    public interface IDbConnectionStringProvider
    {
        public string ProvideFor(string databaseName);
        public int DataSourceCount();
    }
}
