using AutoMapper;
using System.Buffers;
using UsersRestApi.Database.Entities;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ImageService;

namespace UsersRestApi.Services.ProductService
{
    public class ProductsService
    {
        private IProductRepository _repository;
        private ImagesService _imagesService;
        private IMapper _mapper;
        private ILogger<ProductsService> _logger;

        public ProductsService(IProductRepository repository,
                               IMapper mapper,
                               ILogger<ProductsService> logger,
                               ImagesService imagesService)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _imagesService = imagesService;
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

        public async Task<List<OperationStatusResponseBase>> CreateProduct(ProductPostDto productDto)
        {
            var operationStatuses = new List<OperationStatusResponseBase>();
            try
            {             
                var productEntity = _mapper.Map<ProductPostDto, ProductEntity>(productDto);
                var result = await _repository.Create(productEntity);

                operationStatuses.Add(result);

                if (result.Status == StatusName.Error || result.Status == StatusName.Warning)
                    return operationStatuses;


                if (!_imagesService.CreateMainDirectory(productDto))
                {
                    operationStatuses.Add(OperationStatusResonceBuilder
                    .CreateStatusWarning("A repository with the same name already exists for this product"));
                    return operationStatuses;
                }
                    

                result = _imagesService.CreatePreviewImage(productDto);
                operationStatuses.Add(result);
                result = _imagesService.CreateImages(productDto);
                operationStatuses.Add(result);

                return operationStatuses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                operationStatuses.Add(OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("[ProductDto] entity mapping error in [Product] Most likely, the structure of the supplied entity did not match the structure of [ProductDto]",
                    StatusName.Error, null));
                return operationStatuses;
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
