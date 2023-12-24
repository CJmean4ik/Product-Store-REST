using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using ProductAPI.Repositories.Interfaces;
using UsersRestApi.Database.EF;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Implementers
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private DatabaseContext _db;

        public FavoritesRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<OperationStatusResponseBase> Create(FavoritesEntity? entity)
        {
            try
            {
                await _db.Favorites.AddAsync(entity!);
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The product has been added to favorites");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public async Task<OperationStatusResponseBase> Delete(FavoritesEntity? entity)
        {
            try
            {
                var favoriteProduct = await _db.Favorites
                                               .Where(w => w.ProductId == entity.ProductId && w.BuyerId == entity.BuyerId)
                                               .FirstOrDefaultAsync();

                if (favoriteProduct is null)
                    return OperationStatusResonceBuilder.CreateStatusWarning("Cannot remove favorite product fron db");

                _db.Favorites.Remove(entity!);
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The product has been removed from favorites");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public async Task<List<FavoritesEntity>> GetAllByBuyerId(int buyerId)
        {
            var favourites = await _db.Favorites.Where(w => w.BuyerId == buyerId)
                .Include(i => i.Product)
                .ToListAsync();

            return favourites;
        }
    }
}
