using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UsersRestApi.Database.Entities;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ImageParserService;

namespace UsersRestApi.Controllers
{
    public class ProductImageControler : Controller
    {
        private IImageParser<FileStream, OperationStatusResponseBase> _imageParser;
        private ImageConfig _imageConfig;
        private IProductRepository _repository;

        public ProductImageControler(IImageParser<FileStream, OperationStatusResponseBase> imageParser,
                                    IOptions<ImageConfig> imageConfig,
                                    IProductRepository repository)
        {
            _imageParser = imageParser;
            _imageConfig = imageConfig.Value;
            _repository = repository;
        }

        [HttpGet("api/v1/products/images/preview")]
        public async Task<ActionResult<OperationStatusResponseBase>> GetImage()
        {
            string? Id = HttpContext.Request.Query["_product-id"];
            string path;
            string productName;
            string previewName;
            ProductEntity? product;
            OperationStatusResponseBase result;

            if (Id is null)
                return OperationStatusResonceBuilder
                    .CreateStatusWarning("The query string was not entered correctly");

            if (!int.TryParse(Id, out int productId))
                return OperationStatusResonceBuilder
                    .CreateStatusWarning("Invalid id format");


            product = await _repository.GetById(productId);

            if (product is null)
                return OperationStatusResonceBuilder
                    .CreateStatusWarning($"The user could not be found under the id [{productId}]");

            productName = product.Name;
            previewName = product.PreviewImageName;

            path = _imageConfig.ProductPreviewPath
                               .Replace("PRODUCT_NAME",productName)
                               .Replace("FILE_NAME",previewName);

            result = await _imageParser.GetImageAsync(path);

            if (result.Status == StatusName.Warning || result.Status == StatusName.Error)
                return result;

            var operationResult = (OperationStatusResponse<byte[]>)result;

            return File(operationResult.Body!,"application/octet-stream",fileDownloadName: previewName);
            
        }
    }
}
