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
        private IImageReposiroty<IFormFile, OperationStatusResponseBase> _imageRepository;
        private ImageConfig _imageConfig;
        private IProductRepository _repository;

        public ImagesService(IImageReposiroty<IFormFile, OperationStatusResponseBase> imageReposiroty,
                            IOptions<ImageConfig> imageConfig,
                            IProductRepository repository)
        {
            _imageRepository = imageReposiroty;
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

            result = await _imageRepository.GetImageAsync(path);


            return (result, previewName);
        }

        public OperationStatusResponseBase CreatePreviewImage(ProductPostDto productPost)
        {
            _imageConfig.CreatePreviewDirectory(productPost.Name);

            var fileName = Path.GetFileName(productPost.PreviewImage.FileName);
            var path = _imageConfig.ProductPreviewPath
                .Replace("PRODUCT_NAME", productPost.Name)
                .Replace("FILE_NAME", fileName);

            var result = _imageRepository.CreateImage(productPost.PreviewImage, path);
            return result;
        }
        public OperationStatusResponseBase CreateImages(ProductPostDto productPost)
        {     
            _imageConfig.CreateImageDirectory(productPost.Name);

            var result = _imageRepository.CreateImages(productPost.Images, productPost.Name);
            return result;
        }

        public OperationStatusResponseBase RemoveAllImages(ProductDelDto productDel)
        {
            string path = _imageConfig.ProductPath.Replace("FOR_RAPLACE", productDel.Name);
            var result = _imageRepository.RemoveImages(path);
            return result;
        }

        public bool CreateMainDirectory(ProductPostDto productPost)
        {
            if (!_imageConfig.CreateProductDirectory(productPost.Name))
                return false;
            return true;
        }
    }
}
