
namespace DbConnectionProvider.Abstractions
{
    public interface IDbContextProvider<T>
    {
        T ProvideTransactionScoped();
        T ProvideTransactionless();
    }
}
