using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Product;
using ProductAPI.Services;
using UsersRestApi.Models;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Controllers.ProductControllers
{
    [ApiController]
    [Authorize(Roles = "admin, contentMaker")]
    public class ProductsController : Controller
    {
        private ProductsService _productsService;
        private IMapper _mapper;
        public ProductsController(ProductsService productsService, IMapper mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }


        [HttpGet("api/v1/products")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                List<Product> products;
                string NOT_FOUND_TEXT = "Products not found";
                var queryParametrs = Request.Query;

                if (queryParametrs.ContainsKey("id"))
                {
                    if (!int.TryParse(queryParametrs["id"], out int id)) return Json("id is not a number");

                    products = await _productsService.GetProducts(id: id);
                    return products is null ? Json($"Product by id: {id} not found") : products;
                }

                if (queryParametrs.ContainsKey("_limit"))
                {
                    if (!int.TryParse(queryParametrs["_limit"], out int limit)) return Json("limit is not a number");

                    products = await _productsService.GetProducts(limit: limit);
                    return products is null ? Json(NOT_FOUND_TEXT) : products;
                }

                products = await _productsService.GetProducts();
                return products is null ? Json(NOT_FOUND_TEXT) : products;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponseBase>> PostProduct([FromForm] ProductPostDto product)
        {
            var result = await _productsService.CreateProduct(product);
            return Json(result);
        }

        [HttpDelete("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponseBase>> DeleteProduct([FromBody] ProductDelDto product)
        {
            var resultProductService = await _productsService.RemoveProduct(product);

            return Json(resultProductService);
        }

        [HttpPut("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponseBase>> PutProduct([FromBody] ProductPutDto product)
        {
            var result = await _productsService.UpdateProduct(product);
            return Json(result);
        }

    }
}
