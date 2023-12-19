using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Carts;
using ProductAPI.Services.CartService;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Controllers
{
    [ApiController]
    public class ProductCartController : Controller
    {
        private ProductCartService _productCartService;

        public ProductCartController(ProductCartService productCartService)
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

            if (User.Identity.IsAuthenticated)
            {
                //Добавление товара в таблицу carts, в Бд только для зарегистрированных пользователей 
                //Метод по добавлению товара в сессию для корзины 
            }

            //Метод по получению товара из сесси для незарегистрированных пользователей 
            //Товара в корзине может не быть из-за истекшей сессии 
            throw new NotImplementedException();
        }


        [HttpDelete("api/v1/products/carts")]
        public async Task<ActionResult> RemoveProductFromCarts()
        {

            if (User.Identity.IsAuthenticated)
            {
                //Добавление товара в таблицу carts, в Бд только для зарегистрированных пользователей 
                //Метод по добавлению товара в сессию для корзины 
            }

            //Метод по получению товара из сесси для незарегистрированных пользователей 
            //Товара в корзине может не быть из-за истекшей сессии 
            throw new NotImplementedException();
        }

    }
}
