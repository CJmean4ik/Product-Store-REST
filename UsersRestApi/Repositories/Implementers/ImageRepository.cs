using Microsoft.Extensions.Options;
using UsersRestApi.Models;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Implementers
{
    public class ImageRepository : IImageReposiroty<IFormFile, OperationStatusResponseBase>
    {
        private ImageConfig _imageConfig;

        public ImageRepository(IOptions<ImageConfig> imageConfig)
        {
            _imageConfig = imageConfig.Value;
        }
        public OperationStatusResponseBase CreateImage(IFormFile file, string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path: path, FileMode.CreateNew))
                {
                    file.CopyTo(fileStream);
                }
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("The image has been saved");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public OperationStatusResponseBase CreateImages(List<IFormFile> files, string productName)
        {
            foreach (var file in files)
            {
                var path = _imageConfig.ProductImagesPath
                    .Replace("PRODUCT_NAME", productName)
                    .Replace("FILE_NAME", file.FileName);


                CreateImage(file, path);
            }
            return OperationStatusResonceBuilder.CreateStatusSuccessfully("The image has been saved");
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

        public OperationStatusResponseBase RemoveImages(string path)
        {
            if (!Directory.Exists(path))           
                return OperationStatusResonceBuilder.CreateStatusWarning("Directory not founded");

            Directory.Delete(path,true);
            return OperationStatusResonceBuilder.CreateStatusSuccessfully("Directory with images has been removed");
        }

        public OperationStatusResponseBase UpdateImages(string path)
        {
            throw new NotImplementedException();
        }
    }
}
