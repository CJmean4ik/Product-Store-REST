using Microsoft.Extensions.Options;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Implementers
{
    public class ImageRepository : IImageReposiroty<ProductPostDto, OperationStatusResponseBase>
    {
        private ImageConfig _imageConfig;

        public ImageRepository(IOptions<ImageConfig> imageConfig)
        {
            _imageConfig = imageConfig.Value;
        }
        public OperationStatusResponseBase CreateImage(ProductPostDto product,string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path: path, FileMode.CreateNew))
                {
                    product.PreviewImage.CopyTo(fileStream);
                }
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The image has been saved");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }

        public async Task<OperationStatusResponseBase> GetImageAsync(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return OperationStatusResonceBuilder.CreateStatusWarning("File doesnt exist or wrong path!");


                var imageDate = await File.ReadAllBytesAsync(path);

                return OperationStatusResonceBuilder
                    .CreateCustomStatus("The image was successfully received", StatusName.Successfully, imageDate);
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex: ex);
            }
        }

        public OperationStatusResponseBase RemoveImage(string path)
        {
            throw new NotImplementedException();
        }

        public OperationStatusResponseBase UpdateImage(string path)
        {
            throw new NotImplementedException();
        }
    }
}
