using ProductAPI.Database.Entities;
using ProductAPI.Repositories.Interfaces.Operations;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IFavoritesRepository :
        ICreator<OperationStatusResponseBase,FavoritesEntity>,
        IDeleter<OperationStatusResponseBase, FavoritesEntity>
    {
        Task<List<FavoritesEntity>> GetAllByBuyerId(int buyerId);
    }
}
