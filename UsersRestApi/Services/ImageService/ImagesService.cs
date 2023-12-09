using Microsoft.Extensions.Options;
using ProductAPI.DTO;
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
        public OperationStatusResponseBase CreatePreviewImage(ImagePostDto imagePost)
        {
            _imageConfig.CreatePreviewDirectory(imagePost.ProductName);

            var fileName = Path.GetFileName(imagePost.PreviewImage.FileName);
            var path = _imageConfig.ProductPreviewPath
                .Replace("PRODUCT_NAME", imagePost.ProductName)
                .Replace("FILE_NAME", fileName);

            var result = _imageRepository.CreateImage(imagePost.PreviewImage, path);
            return result;
        }
        public OperationStatusResponseBase CreateImages(ImagePostDto imagePost)
        {     
            _imageConfig.CreateImageDirectory(imagePost.ProductName);

            var result = _imageRepository.CreateImages(imagePost.Images, imagePost.ProductName);
            return result;
        }

        public OperationStatusResponseBase RemovePreviewImage(ImageDelDto imageDel)
        {
            string path = _imageConfig.ProductPreviewPath
                                      .Replace("PRODUCT_NAME", imageDel.ProductName)
                                      .Replace("FILE_NAME",imageDel.PreviewImageName);
            var result = _imageRepository.RemoveImageFile(path);
            return result;
        }
        public OperationStatusResponseBase RemoveImages(ImageDelDto imageDel)
        {
            string[]? fileNames = imageDel.AllImagesName;

            if (fileNames == null)          
               return OperationStatusResonceBuilder.CreateStatusWarning(" There is no white image to delete");


            OperationStatusResponseBase result = new OperationStatusResponse<string>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                string path = _imageConfig.ProductPreviewPath
                                     .Replace("PRODUCT_NAME", imageDel.ProductName)
                                     .Replace("FILE_NAME", imageDel.PreviewImageName);
              result = _imageRepository.RemoveImageFile(path);
            }
           
            return result;
        }
        public OperationStatusResponseBase RemoveAllImages(string productName)
        {
            string path = _imageConfig.ProductPath.Replace("FOR_RAPLACE", productName);
            var result = _imageRepository.RemoveImageDirectory(path);
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
