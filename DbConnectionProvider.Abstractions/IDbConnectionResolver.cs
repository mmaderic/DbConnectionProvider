using System.Data;

namespace DbConnectionProvider.Abstractions
{
    /// <summary>
    /// Defines database connection resolver which distincts and returns connection provider defined by string identifier   
    /// </summary>
    public interface IDbConnectionResolver
    {
        public IDbConnectionProvider ResolveFor(string identifier);
        public void CloseConnectionFor(string identifier);
        public void CloseAllConnections();

        public IDbConnectionProvider<TConnection, TTransaction> ResolveFor<TConnection, TTransaction>(string identifier)
            where TConnection : IDbConnection
            where TTransaction : IDbTransaction;    
    }
}
