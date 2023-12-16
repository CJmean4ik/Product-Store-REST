using Microsoft.Extensions.Options;
using ProductAPI.DTO.Image;
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
        public OperationStatusResponseBase CreateImage(IFormFile file,string path, bool creatCopyIfExist = false)
        {
            try
            {
                if (File.Exists(path) && creatCopyIfExist)           
                    return OperationStatusResonceBuilder
                        .CreateStatusWarning($"Same image with name: {file.FileName} alredy exist in directory");                             

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
