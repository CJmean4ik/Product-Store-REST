namespace ProductAPI.Repositories.Interfaces.Operations
{
    public interface IUpdater<R,T>
    {
        Task<R> Update(T? entity);
    }
}
