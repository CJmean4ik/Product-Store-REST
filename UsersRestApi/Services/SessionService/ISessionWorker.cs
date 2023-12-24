namespace ProductAPI.Services.SessionService
{
    public interface ISessionWorker<R>
    {
        List<T>? GetEntitiesByKey<T>(HttpContext context, string key);
        R BindEntitiesInSession<T>(HttpContext context, List<T> entities, string key);
    }
}
