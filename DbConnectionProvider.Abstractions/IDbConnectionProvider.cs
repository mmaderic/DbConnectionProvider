using System.Data;

namespace DbConnectionProvider.Abstractions
{
    public interface IDbConnectionProvider<T>
    where T : IDbConnection
    {
        public T Provide();
        public void Close();
    }
}
