using AutoMapper;
using Microsoft.Extensions.Options;
using ProductAPI.DTO.Image;
using UsersRestApi.Database.Entities;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Services.ImageService
{
    public class ImagesService
    {
        private IImageReposiroty<IFormFile, OperationStatusResponseBase, ImagePutDto> _imageRepository;
        private ImageConfig _imageConfig;
        private IProductRepository _repository;
        private IMapper _mapper;

        public ImagesService(IImageReposiroty<IFormFile, OperationStatusResponseBase, ImagePutDto> imageReposiroty,
                            IOptions<ImageConfig> imageConfig,
                            IProductRepository repository,
                            IMapper mapper)
        {
            _imageRepository = imageReposiroty;
            _imageConfig = imageConfig.Value;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<OperationStatusResponseBase>> CreateImage(ImagePostDto imagePost)
        {
            var results = new List<OperationStatusResponseBase>();

            if (imagePost.Preview != null && imagePost.Images.Count != 0)
            {
                results.Add(createPreviewImage(imagePost.Preview));
                results.AddRange(await createCollectionImages(imagePost));
                return results;
            }

            if (imagePost.Images.Count != 0)
                results.AddRange(await createCollectionImages(imagePost));

            return results;
        }
        private OperationStatusResponseBase createPreviewImage(IFormFile preview)
        {
            var result = _imageRepository.CreateImage(preview);
            return result;
        }
        private async Task<List<OperationStatusResponseBase>> createCollectionImages(ImagePostDto imagePost)
        {
            var results = new List<OperationStatusResponseBase>();
            var addedImage = new List<ImageEntity>();

            foreach (var file in imagePost.Images)
            {
                var addedImageResult = _imageRepository.CreateImage(file);
                if (addedImageResult.Status == StatusName.Successfully)
                {
                    addedImage.Add(new ImageEntity
                    {
                        ImageName = file.FileName
                    });
                }
                results.Add(addedImageResult);
            }

            var result = await _repository.AddImages(imagePost.ProductId, addedImage);

            results.Add(result);

            return results;
        }
        public async Task<List<OperationStatusResponseBase>> RemoveImage(ImageDelDto imageDel)
        {
            var imagesForRemoving = new List<string>();

            var results = new List<OperationStatusResponseBase>();

            foreach (var imageName in imageDel.ImageNames)
            {
                string path = _imageConfig.ProductPath
                                     .Replace("FILE_NAME", imageName);
                var result = _imageRepository.RemoveImageFile(path);

                if (result.Status == StatusName.Successfully)
                    imagesForRemoving.Add(imageName);

                results.Add(result);
            }

            if (imagesForRemoving.Count == 0)
                return results;

            var repositoryResult = await _repository.RemoveImages(imageDel.ProductId, imagesForRemoving);
            results.Add(repositoryResult);

            return results;
        }

        public async Task<List<OperationStatusResponseBase>> UpdateImages(ImagePutDto imagePut)
        {
            var results = new List<OperationStatusResponseBase>();
            
                var path = _imageConfig.ProductPath
                                       .Replace("FILE_NAME", imagePut.OldImageName);
                var result = _imageRepository.RemoveImageFile(path);

                results.Add(result);

                if (result.Status == StatusName.Successfully)              
                    result = _imageRepository.CreateImage(imagePut.NewImage!);                               
                      
            
            results.Add(await _repository.UpdateImages(imagePut.ProductId, imagePut.OldImageName!, imagePut.NewImage!.FileName));
            return results;
        }
    }
}
