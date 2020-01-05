
namespace DbConnectionProvider.Configuration
{
    public class ConnectionStringConfiguration
    {
        public string Identifier { get; set; }
        public string ConnectionString { get; set; }

        public ConnectionStringConfiguration(string identifier, string connectionString)
        {
            Identifier = identifier;
            ConnectionString = connectionString;
        }
    }
}
