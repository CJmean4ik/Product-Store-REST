using Microsoft.Extensions.Options;
using UsersRestApi.Database.Entities;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Services.ImageService
{
    public class ImagesService
    {
        private IImageReposiroty<ProductPostDto, OperationStatusResponseBase> _imageReposiroty;
        private ImageConfig _imageConfig;
        private IProductRepository _repository;

        public ImagesService(IImageReposiroty<ProductPostDto, OperationStatusResponseBase> imageReposiroty,
                            IOptions<ImageConfig> imageConfig,
                            IProductRepository repository)
        {
            _imageReposiroty = imageReposiroty;
            _imageConfig = imageConfig.Value;
            _repository = repository;
        }

        public async Task<(OperationStatusResponseBase responce, string imageName)> GetImage(int productId)
        {
            string path;
            string productName;
            string previewName;
            ProductEntity? product;
            OperationStatusResponseBase result;

            product = await _repository.GetById(productId);

            if (product is null)
                return (OperationStatusResonceBuilder
                    .CreateStatusWarning($"The user could not be found under the id [{productId}]"), "");

            productName = product.Name;
            previewName = product.PreviewImageName;

            path = _imageConfig.ProductPreviewPath
                               .Replace("PRODUCT_NAME", productName)
                               .Replace("FILE_NAME", previewName);

            result = await _imageReposiroty.GetImageAsync(path);


            return (result, previewName);
        }

        public OperationStatusResponseBase CreatePreviewImage(ProductPostDto productPost)
        {
            if (!_imageConfig.CreateProductDirectory(productPost.Name))
                return OperationStatusResonceBuilder
                    .CreateStatusWarning("A repository with the same name already exists for this product");
            _imageConfig.CreatePreviewDirectory(productPost.Name);

            var fileName = Path.GetFileName(productPost.PreviewImage.FileName);
            var path = _imageConfig.ProductPreviewPath
                .Replace("PRODUCT_NAME", productPost.Name)
                .Replace("FILE_NAME", fileName);

            var result = _imageReposiroty.CreateImage(productPost, path);
            return result;
        }
        public async Task<OperationStatusResponseBase> CreateImages(ProductPostDto productPost)
        {
            _imageConfig.CreateImageDirectory(productPost.Name);
        }
    }
}
