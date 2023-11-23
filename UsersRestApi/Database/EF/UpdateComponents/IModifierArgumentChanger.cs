using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.EF.UpdateComponents.Arguments;

namespace UsersRestApi.Database.EF.UpdateComponents
{
    public interface IModifierArgumentChanger<T, V>
        where T : class, new()
        where V : DbContext, new()
    {
        Dictionary<Func<T, T, bool>, ModifierArgumentsBase<T, V>> Tracker { get; set; }

        void InitializeTracker();

        void SearchModifieArguments(T oldEntity, T newEntity);
        void ChangeFoundModifieArguments(T oldEntity, V db);
        Task SaveChangesAsync(V context);
    }
}
