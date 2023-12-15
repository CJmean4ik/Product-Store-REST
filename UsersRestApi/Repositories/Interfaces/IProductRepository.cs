using UsersRestApi.Database.Entities;
using UsersRestApi.Models;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories
{
    public interface IProductRepository : IRepository<ProductEntity, OperationStatusResponseBase>
    {
        Task<ProductEntity?> GetById(int id);
        Task<List<ProductEntity>?> GetLimint(int limit);

        Task<OperationStatusResponseBase> AddImages(int productId, List<ImageEntity> images);
        Task<OperationStatusResponseBase> UpdateImages(int productId, string oldName, string newName);
        Task<OperationStatusResponseBase> RemoveImages(int? productId, List<string> imageName);
        
    }
}
