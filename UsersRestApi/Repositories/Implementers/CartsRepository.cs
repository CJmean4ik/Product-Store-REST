using ProductAPI.Database.Entities;
using ProductAPI.Repositories.Interfaces;
using UsersRestApi.Database.EF;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Implementers
{
    public class CartsRepository : ICartsRepository
    {
        private DatabaseContext _db;

        public CartsRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<OperationStatusResponseBase> Create(CartEntity? entity)
        {
            try
            {
                _db.Carts.Add(entity);
                await _db.SaveChangesAsync();

                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The product was added to the cart");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }

        public Task<OperationStatusResponseBase> Delete(CartEntity? entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<CartEntity>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OperationStatusResponseBase> Update(CartEntity? entity)
        {
            throw new NotImplementedException();
        }
    }
}
