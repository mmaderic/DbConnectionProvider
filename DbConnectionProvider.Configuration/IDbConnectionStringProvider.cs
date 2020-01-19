
namespace DbConnectionProvider.Extensions.Configuration
{
    public interface IDbConnectionStringProvider
    {
        public string ProvideFor(string databaseName);
        public int DataSourceCount();
    }
}
