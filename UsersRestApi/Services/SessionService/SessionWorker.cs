using System.Text.Json;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services.SessionService
{
    public class SessionWorker : ISessionWorker<OperationStatusResponseBase>
    {
        public OperationStatusResponseBase BindEntitiesInSession<T>(HttpContext context, List<T> entities, string key)
        {
            var entitiesJson = JsonSerializer.Serialize(entities);
            context.Session.SetString(key, entitiesJson);

            return OperationStatusResonceBuilder.CreateStatusSuccessfully("Entity have been saved in session");
        }

        public List<T>? GetEntitiesByKey<T>(HttpContext context, string key)
        {
            if (!context.Session.IsAvailable)
                return null;

            if (!context.Session.TryGetValue(key, out byte[]? items))
                return new List<T>();

            var entities = JsonSerializer.Deserialize<List<T>>(items);

            return entities;
        }
    }
}
