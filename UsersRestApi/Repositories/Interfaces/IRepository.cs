namespace UsersRestApi.Repositories
{
    public interface IRepository<T, R> where T : class, new()
    {
        Task<R> Create(T? entity);
        Task<R> Update(T? entity);
        Task<R> Delete(T? entity);
    }

    public interface IRepository<T> : IRepository<T,bool> 
                            where T : class,new()
    {
    }
}
