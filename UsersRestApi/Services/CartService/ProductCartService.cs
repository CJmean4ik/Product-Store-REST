using AutoMapper;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Carts;
using ProductAPI.Repositories.Interfaces;
using System.Security.Claims;
using System.Text.Json;
using UsersRestApi.Database.Entities;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services.CartService
{
    public class ProductCartService
    {
        private ICartsRepository _cartsRepository;

        public ProductCartService(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
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
                var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var buyerId = int.Parse(buyerClaims!.Value);
                var cart = new CartEntity()
                {
                    Buyer = new BuyerEntity
                    {
                        Id = buyerId
                    },
                    Product = new ProductEntity
                    {
                        ProductId = productCartsPost.ProductId
                    }
                };

                var result = await _cartsRepository.Create(cart);
                return result;
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }

        }

        private List<ProductCartsPostDto>? TakeOldProductFromCart(HttpContext httpContext)
        {
            if (!httpContext.Session.IsAvailable)           
                return null;
            

            if (!httpContext.Session.TryGetValue(".Products-in-carts", out byte[]? productsJson))
                return new List<ProductCartsPostDto>();

            var products = JsonSerializer.Deserialize<List<ProductCartsPostDto>>(productsJson);
            httpContext.Session.Remove(".Products-in-carts");

            return products;
        }
        private OperationStatusResponseBase BindAllProductForCart(HttpContext httpContext, List<ProductCartsPostDto> products)
        {
            var productsJson = JsonSerializer.Serialize<List<ProductCartsPostDto>>(products);
            httpContext.Session.SetString(".Products-in-carts", productsJson);

            return OperationStatusResonceBuilder.CreateStatusSuccessfully("Products for the shopping cart have been added to the session");
        }
    }
}
