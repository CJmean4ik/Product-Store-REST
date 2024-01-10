using ProductAPI.Database.Entities;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;

namespace ProductAPI.Database.EF.UpdateComponents.Order
{
    public interface IOrderModifireArgumentChanger : IModifierArgumentChanger<OrderEntity, DatabaseContext>
    {
    }
}
