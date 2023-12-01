using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Services.ImageParserService
{
    public class ImageParser : IImageParser<FileStream, OperationStatusResponseBase>
    {
        public OperationStatusResponseBase CreateImage(FileStream image, string path)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationStatusResponseBase> GetImageAsync(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return OperationStatusResonceBuilder.CreateStatusWarning("File doesnt exist or wrong path!");


                var imageDate = await File.ReadAllBytesAsync(path);

                return OperationStatusResonceBuilder
                    .CreateCustomStatus<byte[]>("The image was successfully received", StatusName.Successfully, imageDate);
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex: ex);
            }
        }
    }
}
