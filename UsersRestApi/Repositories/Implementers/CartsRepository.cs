using Microsoft.EntityFrameworkCore;
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

        public async Task<OperationStatusResponseBase> Delete(CartEntity? entity)
        {
            try
            {
                

                var cart = await _db.Carts.Where(w => w.ProductId == entity.ProductId && w.BuyerId == entity.BuyerId)
                                    .FirstOrDefaultAsync();

                if (cart is null)  
                    return OperationStatusResonceBuilder.CreateStatusWarning("This cart can was not found for deletion");
                

                _db.Carts.Remove(cart);
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("Product was removed from cart this buyer");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }

        public Task<List<CartEntity>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<CartEntity>> GetAllById(int buyerId)
        {
            try
            {
                var cart = await _db.Carts.Where(w => w.BuyerId == buyerId)
                    .Include(i => i.Buyer)
                    .Include(i => i.Product)
                    .ToListAsync();

                return cart;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OperationStatusResponseBase> Update(CartEntity? entity)
        {
            try
            {
                var cart = await _db.Carts.Where(w => w.ProductId == entity.ProductId && w.BuyerId == entity.BuyerId)
                                    .FirstOrDefaultAsync();

                if (cart == null)                
                    return OperationStatusResonceBuilder.CreateStatusWarning("Cart is empty");
                
                cart.Count += entity.Count;

                _db.Entry<CartEntity>(cart).Property(p => p.Count).IsModified = true;
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("Count for this product was updated");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
    }
}
