namespace ProductAPI.Repositories.Interfaces.Operations
{
    public interface IDeleter<R,T>
    {
        Task<R> Delete(T? entity);
    }
}
