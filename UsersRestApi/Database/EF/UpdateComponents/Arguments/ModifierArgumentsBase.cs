using Microsoft.EntityFrameworkCore;

namespace UsersRestApi.Database.EF.UpdateComponents.Arguments
{
    public abstract class ModifierArgumentsBase<T, V>
            where T : class, new()
            where V : DbContext, new()
    {
        public bool IsModified { get; set; }
        public Action<T, T> ValueChanger { get; set; }
        public Action<T, V> Attacher { get; set; }
    }
}
