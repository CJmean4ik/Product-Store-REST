using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Image;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ImageService;

namespace UsersRestApi.Controllers
{
    public class ProductImageControler : Controller
    {
        private ImagesService _imagesService;

        public ProductImageControler(ImagesService imagesService, IProductRepository productRepository)
        {
            _imagesService = imagesService;
        }

        [HttpPost("api/v1/products/images")]
        public async Task<ActionResult<OperationStatusResponseBase>> CreateImage([FromForm] ImagePostDto imagePost)
        {
            var result = await _imagesService.CreateImage(imagePost);
            return Json(result);
        }

        [HttpDelete("api/v1/products/images")]
        public async Task<ActionResult<OperationStatusResponseBase>> DeleteImage([FromForm] ImageDelDto imageDel)
        {
            if (imageDel.ImageNames.Count == 0)
                return OperationStatusResonceBuilder.CreateStatusWarning("Cannot removing images by empty list");

            var result = await _imagesService.RemoveImage(imageDel);
            return Json(result);
        }
           
        [HttpPut("api/v1/products/images")]
        public async Task<ActionResult<OperationStatusResponseBase>> PutCollectionImage([FromForm] ImagePutDto imagePut)
        {       
            return Json("");
        }

    }
}
