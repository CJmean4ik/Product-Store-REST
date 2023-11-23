using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IUserRepository<T,R>
        where T : class, new()
        where R : class
    {
        Task<T> GetByName(string name);
        Task<R> Create(T? entity);
    }
}
