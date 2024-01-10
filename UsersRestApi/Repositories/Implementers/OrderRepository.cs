using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.EF.UpdateComponents.Order;
using ProductAPI.Database.Entities;
using ProductAPI.Repositories.Interfaces.Operations;
using UsersRestApi.Database.EF;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Implementers
{
    public class OrderRepository : IOrderRepository
    {
        private DatabaseContext _db;
        private IOrderModifireArgumentChanger _argumentChanger;

        public OrderRepository(DatabaseContext db, IOrderModifireArgumentChanger argumentChanger)
        {
            _db = db;
            _argumentChanger = argumentChanger;
        }

        public async Task<OperationStatusResponseBase> Create(OrderEntity? entity)
        {
            try
            {
                var res = await _db.Orders.AddAsync(entity);

                await _db.SaveChangesAsync();

                return OperationStatusResonceBuilder.CreateStatusSuccessfully($"Order has been aded. Id: {res.Entity.OrderId}");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex: ex);
            }
        }

        public async Task<OperationStatusResponseBase> CreateOrderProduct(int orderId, Dictionary<int,int> productsCount)
        {
            try
            {
                foreach (var productId in productsCount)
                {
                    var orderEntity = new OrderProductEntity();
                    orderEntity.ProductId = productId.Key;
                    orderEntity.OrderId = orderId;
                    orderEntity.Count = productId.Value;
                    await _db.OrderProducts.AddAsync(orderEntity);
                }

                await _db.SaveChangesAsync();

                return OperationStatusResonceBuilder.CreateStatusSuccessfully($"Order has been aded. Id: {orderId}");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex: ex);
            }
        }
        public async Task<OperationStatusResponseBase> Delete(OrderEntity? entity)
        {
            try
            {
                var order = await _db.Orders.Where(w => w.OrderId == entity.OrderId)
                                            .FirstOrDefaultAsync();

                if (order is null)
                    return OperationStatusResonceBuilder.CreateStatusWarning($"Product order by id: {entity.OrderId} not founded");

                var orderProduct = await _db.OrderProducts.Where(w => w.OrderId == order.OrderId).ToListAsync();

                _db.OrderProducts.RemoveRange(orderProduct);
                
                await _db.SaveChangesAsync();

                return OperationStatusResonceBuilder.CreateStatusSuccessfully("Order have been removed");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }    
        public async Task<List<OrderEntity>> GetByLimit(int limit)
        {
            var orders = await _db.Orders.Take(limit)
                 .Include(i => i.Buyer)
                 .Include(i => i.OrderProducts)
                 .ThenInclude(op => op.Product)
                 .ToListAsync();

            return orders;
        }
        public async Task<OperationStatusResponseBase> Update(OrderEntity? entity)
        {
            try
            {
                var orderFromDb = await _db.Orders
                    .Include(i => i.Buyer)
                    .Where(w => w.OrderId == entity.OrderId)
                    .FirstOrDefaultAsync();

                if (orderFromDb is null)
                {
                    return OperationStatusResonceBuilder
                        .CreateCustomStatus<object>($"Entity for updating by id: [{entity.OrderId}] not found", StatusName.Warning, null);
                }

                _argumentChanger.SearchModifieArguments(orderFromDb, entity);
                _argumentChanger.ChangeFoundModifieArguments(orderFromDb, _db);

                await _argumentChanger.SaveChangesAsync(_db); 
                
                return OperationStatusResonceBuilder.CreateStatusUpdating(entity);
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to update entity. Message: [{ex.Message}]. Reason: [detailed error description]. Time: " + DateTime.Now;             
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }
    }
}
