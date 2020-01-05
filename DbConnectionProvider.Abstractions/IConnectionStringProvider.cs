
namespace DbConnectionProvider.Abstractions
{
    public interface IConnectionStringProvider
    {
        public string ProvideFor(string databaseName);
    }
}
