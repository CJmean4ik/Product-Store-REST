using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;
using UsersRestApi.Database.Entities;

namespace ProductAPI.Database.EF.UpdateComponents.Product
{
    public interface IProductModifierArgumentChanger : IModifierArgumentChanger<ProductEntity, DatabaseContext>
    {
    }
}
