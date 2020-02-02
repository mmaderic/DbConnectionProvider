using Microsoft.Data.SqlClient;

namespace DbConnectionProvider.SqlServer
{
    public class SqlConnectionProvider : ConnectionProvider<SqlConnection, SqlTransaction>
    {
        public SqlConnectionProvider(string connectionString) : base(connectionString)
        {
        }

        public SqlConnectionProvider(string connectionString, string identifier) : base(connectionString, identifier)
        {
        }
    }
}
