using UsersRestApi.Database.Entities;

namespace UsersRestApi.Database.EF.UpdateComponents
{
    public interface IProductModifierArgumentChanger : IModifierArgumentChanger<ProductEntity, DatabaseContext>
    {
        void SearchAndChangeImageModifieArguments(ProductEntity oldEntity, ProductEntity newEntity, DatabaseContext db);
    }
}
