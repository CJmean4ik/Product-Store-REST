using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Orders;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces.Operations;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services
{
    public class OrderService
    {
        private IOrderRepository _orderRepository;
        private IUserRepository<BaseUserEntity, OperationStatusResponseBase> _userRepository;
        private IMapper _mapper;
        public OrderService(IMapper mapper,
                            IOrderRepository orderRepository,
                            IUserRepository<BaseUserEntity, OperationStatusResponseBase> repository)
        {
            _mapper = mapper;
            _userRepository = repository;
            _orderRepository = orderRepository;
        }

        public async Task<ActionResult> GetOrdersByLimmit(int limit)
        {
            try
            {
               var orderEntities = await _orderRepository.GetByLimit(limit);

                if (orderEntities.Count == 0)              
                    return new JsonResult("Count: 0");

                var orders = _mapper.Map<List<OrderEntity>, List<Order>>(orderEntities);

                return new JsonResult(orders);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public async Task<OperationStatusResponseBase> AddOrder(OrderPostDto orderPost)
        {
            var orderEntity = _mapper.Map<OrderPostDto, OrderEntity>(orderPost);
            var userEntity = _mapper.Map<OrderPostDto, BuyerEntity>(orderPost);

            var userResult = await _userRepository.Create(userEntity);

            if (userResult.Status == StatusName.Error)
                return userResult;

            orderEntity.BuyerId = userEntity.Id;

            var result = await _orderRepository.Create(orderEntity);

            result = await _orderRepository.CreateOrderProduct(orderEntity.OrderId,orderPost.ProductsCount);

            return result;
        }
        public async Task<OperationStatusResponseBase> RemoveOrder(int buyerId)
        {
            var userRemovingResult = await _userRepository.Delete(new BuyerEntity { Id = buyerId });

            return userRemovingResult;      
        }
    }
}
