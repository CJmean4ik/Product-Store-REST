﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO;
using UsersRestApi.DTO;
using UsersRestApi.Models;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.ImageService;
using UsersRestApi.Services.ProductService;

namespace UsersRestApi.Controllers
{
    [ApiController]
    //[Authorize]
    public class ProductsController : Controller
    {
        private ProductsService _productsService;
        private ImagesService _imagesService;
        private IMapper _mapper;
        public ProductsController(ProductsService productsService, ImagesService imagesService, IMapper mapper)
        {
            _productsService = productsService;
            _imagesService = imagesService;
            _mapper = mapper;
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
        public async Task<ActionResult<OperationStatusResponseBase>> PostProduct([FromForm] ProductPostDto product)
        {

            var result = await _productsService.CreateProduct(product);

            if (!_imagesService.CreateMainDirectory(product))
            {
                result.Add(OperationStatusResonceBuilder
                .CreateStatusWarning("A repository with the same name already exists for this product"));
                return Json(result);
            }

            var imagePost = _mapper.Map<ProductPostDto, ImagePostDto>(product);

            var resultCreationPreview = await Task.FromResult(_imagesService.CreatePreviewImage(imagePost));
            result.Add(resultCreationPreview);

            var resultCreationCollection = await Task.FromResult(_imagesService.CreateImages(imagePost));
            result.Add(resultCreationCollection);

            return Json(result);
        }

        [HttpDelete("api/v1/products")]
        public async Task<ActionResult<OperationStatusResponseBase>> DeleteProduct([FromBody] ProductDelDto product)
        {
            var resultProductService = await _productsService.RemoveProduct(product);
            if (resultProductService.Status == StatusName.Error || resultProductService.Status == StatusName.Warning)
                return resultProductService;
            var resultImageService = _imagesService.RemoveAllImages(product.Name);
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
