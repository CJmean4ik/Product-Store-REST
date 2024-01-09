using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Favourite;
using ProductAPI.Models;
using ProductAPI.Services;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Controllers.ProductControllers
{
    [ApiController]
    public class ProductFavouritesController : Controller
    {
        private FavoritesService _favouritesService;

        public ProductFavouritesController(FavoritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        [HttpGet("api/v1/products/favourites")]
        public async Task<ActionResult> GetFavouriteProducts()
        {
            try
            {
                if (_favouritesService.TryGetFavoriteProduct(HttpContext, out List<Favourite>? favourites))
                    return Json(new { Favourites = favourites });

                if (User.Identity!.IsAuthenticated)
                {
                    favourites = await _favouritesService.GetFavoriteProductFromDatabase(HttpContext);

                    if (favourites.Count == 0)
                        return Json(new { Favourites = "empty" });

                    return Json(new { Favourites = favourites });
                }

                return Json(new { Favourites = "empty" });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpDelete("api/v1/products/favourites")]
        public async Task<ActionResult> DeleteFavouriteProducts([FromQuery] int productId)
        {
            OperationStatusResponseBase? result = null;
            bool IsAuthenticated = User.Identity!.IsAuthenticated;
        
            result = _favouritesService.RemoveFavoriteProductFromSession(HttpContext, productId);

            if (result.Status != StatusName.Successfully)           
                return Json(result);
           
            if (IsAuthenticated)
                result = await _favouritesService.RemoveFavoriteProductFromDatabase(HttpContext, productId);

            return Json(result);
        }

        [HttpPost("api/v1/products/favourites")]
        public async Task<ActionResult> PostFavouriteProducts([FromBody] ProductFavoritsPostDto favouritsPostDto)
        {
            OperationStatusResponseBase? result = null;
            bool IsAuthenticated = User.Identity!.IsAuthenticated;

            if (!_favouritesService.NonExistFavoriteProduct(HttpContext, favouritsPostDto))
            {
                result = _favouritesService.RemoveFavoriteProductFromSession(HttpContext, favouritsPostDto.ProductId);

                if (IsAuthenticated)
                    result = await _favouritesService.RemoveFavoriteProductFromDatabase(HttpContext, favouritsPostDto.ProductId);

                return Json(new
                {
                    Message = result,
                    product = favouritsPostDto
                });
            }

            result = _favouritesService.AddFavoriteProductInSession(HttpContext, favouritsPostDto);

            if (result.Status != StatusName.Successfully)
                return Json(result);

            if (IsAuthenticated)
                result = await _favouritesService.AddFavoriteProductInDatabase(HttpContext, favouritsPostDto);

            return Json(result);
        }

       
    }
}