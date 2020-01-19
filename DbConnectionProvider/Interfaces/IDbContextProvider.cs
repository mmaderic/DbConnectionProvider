
namespace DbConnectionProvider.Interfaces
{
    public interface IDbContextProvider<T>
    {
        T ProvideTransactionScoped();
        T ProvideTransactionless();
    }
}
