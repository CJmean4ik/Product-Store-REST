using Microsoft.Extensions.Options;
using ProductAPI.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Implementers
{
    public class ImageRepository : IImageReposiroty<IFormFile, OperationStatusResponseBase, ImagePutDto>
    {
        private ImageConfig _imageConfig;

        public ImageRepository(IOptions<ImageConfig> imageConfig)
        {
            _imageConfig = imageConfig.Value;
        }
        public OperationStatusResponseBase CreateImage(IFormFile file)
        {
            try
            {
                string path = _imageConfig.ProductPath.Replace("FILE_NAME", file.FileName);

                using (FileStream fileStream = new FileStream(path: path, FileMode.CreateNew))
                {
                    file.CopyTo(fileStream);
                }
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The image has been saved. Name: " + file.FileName);
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder
                    .CreateStatusError(message: $"Failed to create images with the name: {file.FileName}. Error: " + ex.Message);
            }
        }   
        public OperationStatusResponseBase RemoveImageFile(string path)
        {
            if (!File.Exists(path))
                return OperationStatusResonceBuilder.CreateStatusWarning($"File by path: {path} doesnt found");

            File.Delete(path);
            return OperationStatusResonceBuilder.CreateStatusSuccessfully("File has been removed");
        }

       
    }
}
