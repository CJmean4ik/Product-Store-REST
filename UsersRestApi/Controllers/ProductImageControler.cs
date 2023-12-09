using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ImageService;

namespace UsersRestApi.Controllers
{
    public class ProductImageControler : Controller
    {
        private ImagesService _imagesService;

        public ProductImageControler(ImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet("api/v1/products/images/preview/download")]
        public async Task<ActionResult<OperationStatusResponseBase>> GetImageAnDownload([FromQuery] string? Id)
        {
            var result = await ProccesImage(Id);
            var responce = result.actionResult;


            if (responce.Value != null)
            {
                if (responce.Value.Status == StatusName.Warning || responce.Value.Status == StatusName.Error)
                    return responce.Value;
            }

            HttpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename={result.fileName}");
            return responce;
        }

        [HttpDelete("api/v1/products/images/preview")]
        public ActionResult<OperationStatusResponseBase> RemovePreviewImage([FromBody] ImageDelDto imageDel)
        {      
             var result = _imagesService.RemovePreviewImage(imageDel);
             return Json(result);
        }
        [HttpDelete("api/v1/products/images/collection")]
        public ActionResult<OperationStatusResponseBase> RemoveCollectionImage([FromBody] ImageDelDto imageDel)
        {         
            var result = _imagesService.RemoveImages(imageDel);
            return Json(result);
        }


        private async Task<(ActionResult<OperationStatusResponseBase> actionResult, string fileName)> ProccesImage([FromQuery] string? Id)
        {
            if (Id is null)
                return (OperationStatusResonceBuilder
                        .CreateStatusWarning("The query string was not entered correctly"), "");

            if (!int.TryParse(Id, out int productId))
                return (OperationStatusResonceBuilder
                    .CreateStatusWarning("Invalid id format"), "");

            var result = await _imagesService.GetImage(productId);
            var responce = result.responce;

            if (responce.Status == StatusName.Warning || responce.Status == StatusName.Error)
                return (responce, "");

            var operationResult = (OperationStatusResponse<byte[]>)responce;

            return (File(operationResult.Body!, "image/jpeg", fileDownloadName: result.imageName), result.imageName);
        }
    }
}
