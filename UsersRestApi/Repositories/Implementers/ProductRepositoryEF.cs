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
                    return OperationStatusResonceBuilder
                        .CreateCustomStatus<object>(WARRNING_MESSAGE, StatusName.Warning, null);
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
                return OperationStatusResonceBuilder.CreateCustomStatus<object>(ERROR_MESSAGE, StatusName.Error, null);
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
                    return OperationStatusResonceBuilder
                        .CreateCustomStatus<object>($"Product at id: [{entity.ProductId}] not found", StatusName.Warning, null);
                }

                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusRemoving(entity);
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to create entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogError(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }
        public async Task<List<ProductEntity>?> GetAll()
        {
            try
            {
                var products = await _db.Products
                    .Include(i => i.SubCategory)
                    .Include(i => i.SubCategory.Category)
                    .Include(i => i.Images)
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
        public async Task<ProductEntity> GetById(int id)
        {
            try
            {
                var products = await _db.Products
                    .Include(i => i.SubCategory)
                    .Include(i => i.SubCategory.Category)
                    .Include(i => i.Images)
                    .Where(w => w.ProductId == id)
                    .FirstOrDefaultAsync();

                _logger.LogInformation($"Entity successfully retrieved. Id: [{products.ProductId}]. Time: " + DateTime.Now);
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
                    .Include(i => i.Images)
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
                    return OperationStatusResonceBuilder
                        .CreateCustomStatus<object>($"Entity for updating by id: [{entity.ProductId}] not found", StatusName.Warning, null);
                }

                _argumentChanger.SearchModifieArguments(productFromDb, entity);
                _argumentChanger.ChangeFoundModifieArguments(productFromDb, _db);

                await _argumentChanger.SaveChangesAsync(_db);
                _logger.LogInformation($"Entity successfully updated. Identifier: [{productFromDb.ProductId}]");
                return OperationStatusResonceBuilder.CreateStatusUpdating(entity);
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to update entity. Message: [{ex.Message}]. Reason: [detailed error description]. Time: " + DateTime.Now;
                _logger.LogError(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }
    }

}
