using ProductAPI.Repositories.Interfaces.Operations;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IUserRepository<T,R> : ICreator<R,T>, IDeleter<R,T>
        where T : class, new()
        where R : class
    {
        Task<T> GetByName(string name);
    }
}
