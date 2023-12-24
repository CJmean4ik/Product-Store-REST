using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Carts;
using ProductAPI.Services;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Controllers.ProductControllers
{
    [ApiController]
    public class ProductCartController : Controller
    {
        private CartService _productCartService;

        public ProductCartController(CartService productCartService)
        {
            _productCartService = productCartService;
        }

        [HttpPost("api/v1/products/carts")]
        public async Task<ActionResult<OperationStatusResponseBase>> PostProductInCarts([FromBody] ProductCartsPostDto productCartsPost)
        {
            var result = _productCartService.CreateAndSaveProductInSession(productCartsPost, HttpContext);

            if (result.Status == StatusName.Warning || result.Status == StatusName.Error)
                return result;

            if (User.Identity!.IsAuthenticated)
            {
                result = await _productCartService.AddProductForAuthorizedBuyer(productCartsPost, HttpContext);

                if (result.Status == StatusName.Error)
                    HttpContext.Session.Remove(".Products-in-carts");

                return result;
            }

            return result;
        }

        [HttpGet("api/v1/products/carts")]
        public async Task<ActionResult> GetProductForCarts()
        {
            try
            {
                var product = _productCartService.GetAllProductsFromSession(HttpContext);

                if (product.Count != 0)
                    return Json(product);

                if (User.Identity.IsAuthenticated)
                {
                    var productFromDb = await _productCartService.GetAllProductsForAuthorizedBuyer(HttpContext);

                    if (productFromDb.Count == 0)
                        return Json(new { cart = "empty" });

                    return Json(productFromDb);
                }

                return Json(new { cart = "empty" });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPut("api/v1/products/carts")]
        public async Task<ActionResult> PutProductCountInCarts([FromBody] ProductCartsPutDto cartsPutDto)
        {
            try
            {
                var productFromSession = _productCartService.UpdateProductCountInSession(HttpContext, cartsPutDto);

                if (productFromSession.Status != StatusName.Successfully)
                    return Json(productFromSession);

                if (User.Identity.IsAuthenticated)
                {
                    var result = await _productCartService.UpdateCountProductsForAutorizedBuyer(HttpContext, cartsPutDto);

                    return Json(result);
                }

                return Json(productFromSession);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpDelete("api/v1/products/carts")]
        public async Task<ActionResult> RemoveProductFromCarts([FromQuery] string Id)
        {
            if (!int.TryParse(Id, out int productId))
                return Json("Incorrect Id format");

            var result = _productCartService.RemoveProductCountFromSession(HttpContext, productId);

            if (result.Status != StatusName.Successfully)
                return Json(result);

            if (User.Identity.IsAuthenticated)
            {               
                var productResult = await _productCartService.RemoveProductsForAutorizedBuyer(HttpContext, productId);

                return Json(productResult);
            }

            return Json(result);
        }

    }
}
