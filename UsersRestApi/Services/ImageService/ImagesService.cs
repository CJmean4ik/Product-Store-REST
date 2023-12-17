using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult GetImageByType(string imageType, string imageName)
        {
            if (imageType == "preview")
            {
                string path = _imageConfig.PreviewPath.Replace("FILE_NAME", imageName);

                if (File.Exists(path))
                    return new PhysicalFileResult(path, "image/jpeg");

                return new StatusCodeResult(404);
            }
            if (imageType == "collection")
            {
                string path = _imageConfig.CollectionPath.Replace("FILE_NAME", imageName);

                if (File.Exists(path))
                    return new PhysicalFileResult(path, "image/jpeg");

                return new StatusCodeResult(404);
            }

            return new JsonResult($"Unknown type of image you are trying to request: {imageType}");
        }
        public async Task<List<OperationStatusResponseBase>> CreateImage(ImagePostDto imagePost)
        {
            var results = new List<OperationStatusResponseBase>();

            if (imagePost.Preview != null && imagePost.Images.Count != 0)
            {
                results.Add(createPreviewImage(imagePost.Preview, imagePost.ReplaceImageIfExist));
                results.AddRange(await createCollectionImages(imagePost, imagePost.ReplaceImageIfExist));
                return results;
            }

            if (imagePost.Images.Count != 0)
                results.AddRange(await createCollectionImages(imagePost, imagePost.ReplaceImageIfExist));

            return results;
        }
        private OperationStatusResponseBase createPreviewImage(IFormFile preview, bool replaceImageIfExist)
        {
            string path = _imageConfig.PreviewPath.Replace("FILE_NAME", preview.FileName);
            var result = _imageRepository.CreateImage(preview, path, replaceImageIfExist);
            return result;
        }
        private async Task<List<OperationStatusResponseBase>> createCollectionImages(ImagePostDto imagePost, bool replaceImageIfExist)
        {
            var results = new List<OperationStatusResponseBase>();
            var addedImages = new List<ImageEntity>();

            foreach (var file in imagePost.Images)
            {
                string path = _imageConfig.CollectionPath.Replace("FILE_NAME", file.FileName);
                var addedImageResult = _imageRepository.CreateImage(file, path, replaceImageIfExist);
                if (addedImageResult.Status == StatusName.Successfully)
                {
                    addedImages.Add(new ImageEntity
                    {
                        ImageName = file.FileName
                    });
                }
                results.Add(addedImageResult);
            }

            var result = await _repository.AddImages(imagePost.ProductId, addedImages);

            results.Add(result);

            return results;
        }
        public async Task<List<OperationStatusResponseBase>> RemoveImage(ImageDelDto imageDel)
        {
            var imagesForRemoving = new List<string>();

            var results = new List<OperationStatusResponseBase>();
            removeExistingImages(imageDel, imagesForRemoving, results);

            var repositoryResult = await _repository.RemoveImages(imageDel.ProductId, imagesForRemoving);

            results.Add(repositoryResult);

            return results;
        }
        private void removeExistingImages(ImageDelDto imageDel, List<string> imagesForRemoving, List<OperationStatusResponseBase> results)
        {
            foreach (var imageParams in imageDel.ParamsDelDtos)
            {
                string path;

                if (imageParams.IsPreviewRemoving)
                    path = _imageConfig.PreviewPath.Replace("FILE_NAME", imageParams.ImageName);

                path = _imageConfig.CollectionPath.Replace("FILE_NAME", imageParams.ImageName);

                var result = _imageRepository.RemoveImageFile(path);

                if (result.Status == StatusName.Successfully)
                    imagesForRemoving.Add(imageParams.ImageName);

                results.Add(result);
            }
        }
        public async Task<List<OperationStatusResponseBase>> UpdateImages(ImagePutDto imagePut)
        {
            var results = new List<OperationStatusResponseBase>();
            string path = string.Empty;

            if (imagePut.IsPreviewUpdating)           
                path = _imageConfig.PreviewPath.Replace("FILE_NAME", imagePut.OldImageName);
            
            path = _imageConfig.CollectionPath.Replace("FILE_NAME", imagePut.OldImageName);

            var result = _imageRepository.RemoveImageFile(path);

            results.Add(result);

            if (result.Status == StatusName.Successfully)           
                result = _imageRepository.CreateImage(imagePut.NewImage!, "");

            results.Add(await _repository.UpdateImages(imagePut.ProductId, imagePut.OldImageName!, imagePut.NewImage!.FileName));
            return results;
        }
    }
}
