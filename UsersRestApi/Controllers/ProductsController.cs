using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ProductService;

namespace UsersRestApi.Controllers
{
    [ApiController]
    // [Authorize]
    public class ProductsController : Controller
    {
        private ProductsService _productsService;

        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("api/v1/products")]
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
        public async Task<ActionResult<OperationStatusResponse>> PostProduct([FromBody] ProductPostDto product)
        {
            var result = await _productsService.CreateProduct(product);
            return Json(result);
        }

        [HttpDelete("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteProduct([FromBody] ProductDelDto product)
        {
            var result = await _productsService.RemoveProduct(product);
            return Json(result);
        }
        
        [HttpPut("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponse>> PutProduct([FromBody] ProductPutDto product)
        {
            var result = await _productsService.UpdateProduct(product);
            return Json(result);
        }

    }
}
