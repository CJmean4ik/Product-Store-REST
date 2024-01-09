using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.Orders;
using ProductAPI.Services;

namespace ProductAPI.Controllers.ProductControllers
{
    [ApiController]
    [Authorize(Roles = "admin, manager")]
    public class ProductOrdersController : Controller
    {
        private OrderService _orderService;

        public ProductOrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [AllowAnonymous]
        [HttpGet("api/v1/orders")]
        public async Task<ActionResult> GetOrder([FromQuery] int limit = 10)
        {
            var result = await _orderService.GetOrdersByLimmit(limit);
            return result;
        }

        [HttpPost("api/v1/orders")]
        [AllowAnonymous]
        public async Task<ActionResult> PostOrder([FromBody] OrderPostDto orderPost)
        {
            var result = await _orderService.AddOrder(orderPost);
            return Json(result);
        }

        [HttpDelete("api/v1/orders")]
        [AllowAnonymous]
        public async Task<ActionResult> DelOrder([FromQuery] int buyerId)
        {
           var result = await _orderService.RemoveOrder(buyerId);
           return Json(result);
        }

        [HttpPut("api/v1/orders")]
        public async Task<ActionResult> PutOrder([FromBody] OrderPostDto orderPost)
        {
            throw new NotImplementedException();
        }
    }
}
