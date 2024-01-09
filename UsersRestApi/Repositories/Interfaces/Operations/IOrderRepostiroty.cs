using ProductAPI.Database.Entities;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Interfaces.Operations
{
    public interface IOrderRepository : IRepository<OrderEntity,OperationStatusResponseBase>
    {
        Task<List<OrderEntity>> GetByLimit(int limit);
        Task<OperationStatusResponseBase> CreateOrderProduct(int orderId, Dictionary<int, int> productsCount);
    }
}
