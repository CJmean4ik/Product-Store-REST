using ProductAPI.Database.Entities;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Repositories.Interfaces
{
    public interface ICartsRepository : IRepository<CartEntity,OperationStatusResponseBase>
    {      
    }
}
