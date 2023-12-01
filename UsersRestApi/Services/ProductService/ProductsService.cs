using AutoMapper;
using UsersRestApi.Database.Entities;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Services.ProductService
{
    public class ProductsService
    {
        private IProductRepository _repository;
        private IMapper _mapper;
        private ILogger<ProductsService> _logger;

        public ProductsService(IProductRepository repository, IMapper mapper, ILogger<ProductsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Product>> GetProducts(int id = 0, int limit = 0)
        {
            List<Product> responceList = new List<Product>();

            if (id != 0)
            {
               responceList.Add(await getProductById(id));
               return responceList;
            }

            if (limit != 0) return await getLimitProducts(limit);

            return await getProducts();
        }
        private async Task<List<Product>> getProducts()
        {
            var productsEntities = await _repository.GetAll();

            var products = _mapper.Map<List<ProductEntity>, List<Product>>(productsEntities);

            return products;
        }
        private async Task<Product> getProductById(int id)
        {
            var productEntities = await _repository.GetById(id);

            if (productEntities is null) return null;

            var products = _mapper.Map<ProductEntity, Product>(productEntities);
            return products;
        }
        private async Task<List<Product>> getLimitProducts(int limit)
        {
            var productsEntities = await _repository.GetLimint(limit);

            var products = _mapper.Map<List<ProductEntity>, List<Product>>(productsEntities);
            return products;
        }

        public async Task<OperationStatusResponseBase> CreateProduct(ProductPostDto productDto)
        {
            try
            {
                var productEntity = _mapper.Map<ProductPostDto, ProductEntity>(productDto);
                var result = await _repository.Create(productEntity);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("[ProductDto] entity mapping error in [Product] Most likely, the structure of the supplied entity did not match the structure of [ProductDto]",
                    StatusName.Error, null);
            }
        }
        public async Task<OperationStatusResponseBase> RemoveProduct(ProductDelDto productDto)
        {
            try
            {
                var product = _mapper.Map<ProductDelDto, ProductEntity>(productDto);
                var result = await _repository.Delete(product);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("[ProductDto] entity mapping error in [Product] Most likely, the structure of the supplied entity did not match the structure of [ProductDto]",
                    StatusName.Error, null);
            }
        }
        public async Task<OperationStatusResponseBase> UpdateProduct(ProductPutDto productDto)
        {
            try
            {
                var product = _mapper.Map<ProductPutDto, ProductEntity>(productDto);
                var result = await _repository.Update(product);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("[ProductDto] entity mapping error in [Product] Most likely, the structure of the supplied entity did not match the structure of [ProductDto]",
                    StatusName.Error, null);
            }
        }
    }

}
