using ProductAPI.Database.Entities;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents.Arguments;

namespace ProductAPI.Database.EF.UpdateComponents.Arguments
{
    public class OrderModifierArguments : ModifierArgumentsBase<OrderEntity,DatabaseContext>
    {
    }
}
