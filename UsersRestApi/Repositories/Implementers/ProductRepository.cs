using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;
using UsersRestApi.Database.Entities;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private DatabaseContext _db;
        private ILogger<ProductRepository> _logger;
        private IProductModifierArgumentChanger _argumentChanger;
        public ProductRepository(DatabaseContext db, ILogger<ProductRepository> logger, IProductModifierArgumentChanger argumentChanger)
        {
            _db = db;
            _logger = logger;
            _argumentChanger = argumentChanger;
        }
        #region Product_Manipulation

       
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

                var images = product.Images;

                _db.Products.Remove(product);
                _db.ImageEntities.RemoveRange(images);

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
        #endregion

        #region Product_Image_Manipulation

   
        public async Task<OperationStatusResponseBase> UpdateImages(int productId, string oldName, string newName)
        {
            try
            {
                var imageEntity = await _db.ImageEntities
                    .Where(w => w.ImageName == oldName && w.ProductEntity.ProductId == productId)
                    .FirstOrDefaultAsync();

                if (imageEntity is null)
                {
                    _logger.LogInformation($"Entity has null");
                    return OperationStatusResonceBuilder
                        .CreateCustomStatus<object>($"Entity for updating by id: [{productId}] not found", StatusName.Warning, null);
                }

                imageEntity.ImageName = newName;

                _db.Entry(imageEntity).Property(p => p.ImageName).IsModified = true;
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Entity successfully updated. Identifier: [{imageEntity.ImageId}]");
                return OperationStatusResonceBuilder.CreateStatusUpdating(imageEntity);
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to update entity. Message: [{ex.Message}]. Reason: [detailed error description]. Time: " + DateTime.Now;
                _logger.LogError(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }
        public async Task<OperationStatusResponseBase> AddImages(int productId, List<ImageEntity> images)
        {
            try
            {
                var product = await _db.Products.Where(w => w.ProductId == productId)
                                  .Include(i => i.Images)
                                  .FirstOrDefaultAsync();

                if (product == null)
                    return OperationStatusResonceBuilder
                        .CreateStatusWarning($"Entity by id: [{productId}] not found");

                product.Images.AddRange(images);

                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("Image/Images have been added for product by name: " + product.Name);
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public async Task<OperationStatusResponseBase> RemoveImages(int? productId, List<string> imagesForDeleting)
        {
            try
            {
                var images = await _db.ImageEntities
                    .Where(w => w.ProductEntity.ProductId == productId)
                    .ToListAsync();

                if (images == null)
                    return OperationStatusResonceBuilder
                        .CreateStatusWarning($"Images by id product: [{productId}] not found");

                foreach (var image in images)
                {
                    foreach (var imageForDeleting in imagesForDeleting)
                    {
                        if (image.ImageName == imageForDeleting)
                        {
                            _db.ImageEntities.Remove(image);
                            break;
                        }
                    }
                }
                                       
                await _db.SaveChangesAsync();
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("Image / Images have been removed for product by id: " + productId);
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }

        #endregion
    }

}
