using AutoMapper;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Carts;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using System.Security.Claims;
using System.Text.Json;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services.CartService
{
    public class ProductCartService
    {
        private ICartsRepository _cartsRepository;
        private IMapper _mapper;
        public ProductCartService(ICartsRepository cartsRepository, IMapper mapper)
        {
            _cartsRepository = cartsRepository;
            _mapper = mapper;
        }

        public OperationStatusResponseBase CreateAndSaveProductInSession(ProductCartsPostDto productCartsPost, HttpContext httpContext)
        {
            try
            {
                var products = TakeOldProductFromCart(httpContext);

                if (products is null)
                    return OperationStatusResonceBuilder.CreateStatusWarning("This session is not active for you at the moment");

                products.Add(productCartsPost);
                var result = BindAllProductForCart(httpContext, products);
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
                CartEntity cart = CreateCartEntity(httpContext,productCartsPost.ProductId,productCartsPost.Count);
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
            var result = TakeOldProductFromCart(httpContext);

            if (result is null) throw new SessionNotAvailableExeption();

            if (result.Count == 0) return new List<Cart>();

            return _mapper.Map<List<ProductCartsPostDto>, List<Cart>>(result);
        }
        public async Task<List<Cart>> GetAllProductsForAuthorizedBuyer(HttpContext httpContext)
        {
            var result = TakeOldProductFromCart(httpContext)!;

            if (result.Count != 0)
                return _mapper.Map<List<ProductCartsPostDto>, List<Cart>>(result);

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
            var products = TakeOldProductFromCart(httpContext);

            foreach (var product in products)
            {
                if (product.ProductId == cartsPutDto.ProductId)
                {
                    product.Count += cartsPutDto.Count;
                    break;
                }
            }
            var result = BindAllProductForCart(httpContext, products);
            return result;
        }
        public OperationStatusResponseBase RemoveProductCountFromSession(HttpContext httpContext, int productId)
        {
            var products = TakeOldProductFromCart(httpContext);

            var removed = products!.RemoveAll(m => m.ProductId == productId);
            if (removed == 0)
                return OperationStatusResonceBuilder.CreateStatusWarning("The product could not be deleted from the shopping cart");

            var result = BindAllProductForCart(httpContext, products);
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
        private List<ProductCartsPostDto>? TakeOldProductFromCart(HttpContext httpContext)
        {
            if (!httpContext.Session.IsAvailable)
                return null;

            if (!httpContext.Session.TryGetValue(".Products-in-carts", out byte[]? productsJson))
                return new List<ProductCartsPostDto>();

            var products = JsonSerializer.Deserialize<List<ProductCartsPostDto>>(productsJson);

            return products;
        }
        private OperationStatusResponseBase BindAllProductForCart(HttpContext httpContext, List<ProductCartsPostDto> products)
        {
            var productsJson = JsonSerializer.Serialize<List<ProductCartsPostDto>>(products);
            httpContext.Session.SetString(".Products-in-carts", productsJson);

            return OperationStatusResonceBuilder.CreateStatusSuccessfully("Products for the shopping cart have been added to the session");
        }
        private async Task<List<Cart>> GetAllProductFromDatabase(HttpContext httpContext)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);

            var cart = await _cartsRepository.GetAllById(buyerId);

            if (cart is null || cart.Count == 0) return new List<Cart>();

            var productForSession = _mapper.Map<List<CartEntity>, List<ProductCartsPostDto>>(cart);
            BindAllProductForCart(httpContext, productForSession);

            var cartProducts = _mapper.Map<List<CartEntity>, List<Cart>>(cart);

            return cartProducts;
        }
    }
}
