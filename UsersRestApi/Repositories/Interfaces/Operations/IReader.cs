namespace ProductAPI.Repositories.Interfaces.Operations
{
    public interface IReader<R,T>
    {
        Task<T> Get();
    }
}
