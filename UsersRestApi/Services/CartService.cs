using AutoMapper;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Carts;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.SessionService;
using System.Security.Claims;
using System.Text.Json;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services
{
    public class CartService
    {
        private const string SESSION_KEY = ".Carts";

        private ICartsRepository _cartsRepository;
        private IMapper _mapper;
        private ISessionWorker<OperationStatusResponseBase> _sessionWorker;

        public CartService(ICartsRepository cartsRepository,
            IMapper mapper,
            ISessionWorker<OperationStatusResponseBase> sessionWorker)
        {
            _cartsRepository = cartsRepository;
            _mapper = mapper;
            _sessionWorker = sessionWorker;
        }

        public OperationStatusResponseBase CreateAndSaveProductInSession(ProductCartsPostDto productCartsPost, HttpContext httpContext)
        {
            try
            {
                var products = _sessionWorker.GetEntitiesByKey<Cart>(httpContext, SESSION_KEY);

                if (products is null)
                    return OperationStatusResonceBuilder.CreateStatusWarning("This session is not active or wrong key");

                var productCart = _mapper.Map<ProductCartsPostDto, Cart>(productCartsPost);

                products.Add(productCart);
                var result = _sessionWorker.BindEntitiesInSession(httpContext, products, SESSION_KEY);
                return result;
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public async Task<OperationStatusResponseBase> AddProductForAuthorizedBuyer(ProductCartsPostDto productCartsPost, HttpContext httpContext)
        {
            try
            {
                CartEntity cart = CreateCartEntity(httpContext, productCartsPost.ProductId, productCartsPost.Count);
                var result = await _cartsRepository.Create(cart);
                return result;
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }      
        public List<Cart> GetAllProductsFromSession(HttpContext httpContext)
        {
            var result = _sessionWorker.GetEntitiesByKey<Cart>(httpContext,SESSION_KEY);

            if (result is null) throw new SessionNotAvailableExeption();

            if (result.Count == 0) return result;

            return result;
        }
        public async Task<List<Cart>> GetAllProductsForAuthorizedBuyer(HttpContext httpContext)
        { 
            var productFromDb = await GetAllProductFromDatabase(httpContext);

            return productFromDb;
        }     
        public async Task<OperationStatusResponseBase> UpdateCountProductsForAutorizedBuyer(HttpContext httpContext, ProductCartsPutDto cartsPutDto)
        {
            CartEntity cart = CreateCartEntity(httpContext, cartsPutDto.ProductId, cartsPutDto.Count);

            var result = await _cartsRepository.Update(cart);

            return result;
        }
        public OperationStatusResponseBase UpdateProductCountInSession(HttpContext httpContext, ProductCartsPutDto cartsPutDto)
        {
            var products = _sessionWorker.GetEntitiesByKey<Cart>(httpContext, SESSION_KEY);

            foreach (var product in products)
            {
                if (product.ProductId == cartsPutDto.ProductId)
                {
                    product.Count += cartsPutDto.Count;
                    break;
                }
            }
            var result = _sessionWorker.BindEntitiesInSession(httpContext, products, SESSION_KEY);
            return result;
        }     
        public OperationStatusResponseBase RemoveProductCountFromSession(HttpContext httpContext, int productId)
        {
            var products = _sessionWorker.GetEntitiesByKey<Cart>(httpContext, SESSION_KEY);

            var removed = products!.RemoveAll(m => m.ProductId == productId);
            if (removed == 0)
                return OperationStatusResonceBuilder.CreateStatusWarning("The product could not be deleted from the shopping cart");

            var result = _sessionWorker.BindEntitiesInSession(httpContext, products, SESSION_KEY);
            return result;
        }
        public async Task<OperationStatusResponseBase> RemoveProductsForAutorizedBuyer(HttpContext httpContext, int productId)
        {
            CartEntity cart = CreateCartEntity(httpContext, productId: productId);
            var result = await _cartsRepository.Delete(cart);
            return result;
        }            
        private CartEntity CreateCartEntity(HttpContext httpContext, int productId, int count = 0)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);
            var cart = new CartEntity()
            {
                BuyerId = buyerId,
                ProductId = productId,
                Count = count
            };
            return cart;
        }         
        private async Task<List<Cart>> GetAllProductFromDatabase(HttpContext httpContext)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);

            var cart = await _cartsRepository.GetAllById(buyerId);

            if (cart is null || cart.Count == 0) return new List<Cart>();

            var cartProducts = _mapper.Map<List<CartEntity>, List<Cart>>(cart);

            _sessionWorker.BindEntitiesInSession(httpContext, cartProducts, SESSION_KEY);
         
            return cartProducts;
        }
    }
}
