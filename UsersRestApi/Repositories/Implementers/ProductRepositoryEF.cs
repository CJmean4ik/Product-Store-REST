using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;
using UsersRestApi.Database.Entities;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories
{
    public class ProductRepositoryEF : IProductRepository
    {
        private DatabaseContext _db;
        private ILogger<ProductRepositoryEF> _logger;
        private IProductModifierArgumentChanger _argumentChanger;
        public ProductRepositoryEF(DatabaseContext db, ILogger<ProductRepositoryEF> logger, IProductModifierArgumentChanger argumentChanger)
        {
            _db = db;
            _logger = logger;
            _argumentChanger = argumentChanger;
        }

        public async Task<OperationStatusResponseBase> Create(ProductEntity? entity)
        {
            try
            {
                string CATEGORY_NAME = entity.SubCategory.Name;
                await bindCategoryForProduct(entity);

                if (entity.SubCategory is null)
                {
                    string WARRNING_MESSAGE = $"Cannot bind SubCategory and Category because category [{CATEGORY_NAME}] Not being in the Database Time: " + DateTime.Now;
                    _logger.LogInformation(WARRNING_MESSAGE);
                    return OperationStatusResonceBuilder.CreateCustomStatus(WARRNING_MESSAGE,StatusName.Warning);
                }

                await _db.Products.AddAsync(entity!);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Entity successfully created. Identifier: [{entity!.ProductId}]. Time: " + DateTime.Now);
                return OperationStatusResonceBuilder.CreateStatusAdding($"Identifier: [{entity!.ProductId}]");
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to create entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogWarning(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateCustomStatus(ERROR_MESSAGE, StatusName.Error);
            }
        }
        private async Task bindCategoryForProduct(ProductEntity entity)
        {          
                var category = await _db.SubCategories
                    .Include(ic => ic.Category)
                    .Where(w => w.Name == entity.SubCategory.Name)
                    .FirstOrDefaultAsync();

            entity.SubCategory = category;
        }
        public async Task<OperationStatusResponseBase> Delete(ProductEntity? entity)
        {
            try
            {
                var product = await _db.Products.FindAsync(entity!.ProductId);

                if (product is null)
                {
                    _logger.LogWarning($"Product at id: [{entity.ProductId}] not found");
                    return OperationStatusResonceBuilder.CreateCustomStatus($"Product at id: [{entity.ProductId}] not found",StatusName.Warning);
                }

                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusRemoving();
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to create entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogError(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateCustomStatus(ERROR_MESSAGE, StatusName.Error);
            }
        }
        public async Task<List<ProductEntity>?> GetAll()
        {
            try
            {
                var products = await _db.Products.Include(i => i.SubCategory).Include(i => i.SubCategory.Category).ToListAsync();

                _logger.LogInformation($"Entities successfully retrieved. Count: [{products.Count}]. Time: " + DateTime.Now);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to retrieve entities. Reason: [{ex.Message}]. Time: " + DateTime.Now);
                return null;
            }
        }
        public async Task<List<ProductEntity>> GetById(int id)
        {
            try
            {
                var products = await _db.Products
                    .Include(i => i.SubCategory)
                    .Include(i => i.SubCategory.Category)
                    .Where(w => w.ProductId == id)
                    .ToListAsync();

                _logger.LogInformation($"Entity successfully retrieved. Id: [{products?[0].ProductId}]. Time: " + DateTime.Now);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to retrieve entity. Reason: [{ex.Message}]. Time: " + DateTime.Now);
                return null;
            }
        }
        public async Task<List<ProductEntity>?> GetLimint(int limit)
        {
            try
            {
                var products = await _db.Products
                    .Include(i => i.SubCategory)
                    .Include(i => i.SubCategory.Category)
                    .Take(limit)
                    .ToListAsync();

                _logger.LogInformation($"Entities successfully retrieved. Count: [{products.Count}]. Time: " + DateTime.Now);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to retrieve entities. Reason: [{ex.Message}]. Time: " + DateTime.Now);
                return null;
            }
        }
        public async Task<OperationStatusResponseBase> Update(ProductEntity? entity)
        {
            try
            {
                var productFromDb = await _db.Products.FindAsync(entity.ProductId);
                if (productFromDb is null)
                {
                    _logger.LogInformation($"Entity has null");
                    return OperationStatusResonceBuilder.CreateCustomStatus($"Entity for updating by id: [{entity.ProductId}] not found", StatusName.Warning);
                }

                _argumentChanger.SearchModifieArguments(productFromDb,entity);
                _argumentChanger.ChangeFoundModifieArguments(productFromDb,_db);

                await _argumentChanger.SaveChangesAsync(_db);
                _logger.LogInformation($"Entity successfully updated. Identifier: [{productFromDb.ProductId}]");
                return OperationStatusResonceBuilder.CreateStatusUpdating();
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to update entity. Message: [{ex.Message}]. Reason: [detailed error description]. Time: " + DateTime.Now;
                _logger.LogError(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateCustomStatus(ERROR_MESSAGE, StatusName.Error);
            }
        }       
    }

}
