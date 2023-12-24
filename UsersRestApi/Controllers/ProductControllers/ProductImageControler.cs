using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Image;
using ProductAPI.Services;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Controllers.ProductControllers
{
    [Authorize(Roles = "admin, contentMaker")]
    [ApiController]
    public class ProductImageControler : Controller
    {
        private ImagesService _imagesService;

        public ProductImageControler(ImagesService imagesService, IProductRepository productRepository)
        {
            _imagesService = imagesService;
        }

        [HttpGet("api/v1/products/images/{typeImage}/{imageName}")]
        [AllowAnonymous]
        public IActionResult GetImage(string typeImage, string imageName)
        {
            var result = _imagesService.GetImageByType(typeImage, imageName);
            return result;
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
            if (imageDel.ParamsDelDtos.Count == 0)
                return OperationStatusResonceBuilder.CreateStatusWarning("Cannot removing images by empty list");

            var result = await _imagesService.RemoveImage(imageDel);
            return Json(result);
        }

        [HttpPut("api/v1/products/images")]
        public async Task<ActionResult<OperationStatusResponseBase>> PutCollectionImage([FromForm] ImagePutDto imagePut)
        {
            if (imagePut.NewImage == null || imagePut.OldImageName == "")
                return OperationStatusResonceBuilder.CreateStatusWarning("Cannot updating image by empty file or name");

            var result = await _imagesService.UpdateImages(imagePut);
            return Json(result);
        }

    }
}
