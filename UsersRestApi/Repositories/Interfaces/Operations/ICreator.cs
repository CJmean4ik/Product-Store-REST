namespace ProductAPI.Repositories.Interfaces.Operations
{
    public interface ICreator<R,T>
    {
        Task<R> Create(T? entity);
    }
}
