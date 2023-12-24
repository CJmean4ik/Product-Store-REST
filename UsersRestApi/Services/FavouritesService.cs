using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Favourite;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.SessionService;
using System.Security.Claims;
using UsersRestApi.Repositories.OperationStatus;

namespace ProductAPI.Services
{
    public class FavoritesService
    {
        private const string SESSION_KEY = ".Favourite";

        private IMapper _mapper;
        private IFavoritesRepository _favouritesRepository;
        private ISessionWorker<OperationStatusResponseBase> _sessionWorker;

        public FavoritesService(IMapper mapper,
            IFavoritesRepository favouritesRepository,
            ISessionWorker<OperationStatusResponseBase> sessionWorker)
        {
            _mapper = mapper;
            _favouritesRepository = favouritesRepository;
            _sessionWorker = sessionWorker;
        }

        public async Task<List<Favourite>> GetFavoriteProductFromDatabase(HttpContext httpContext)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);

            var favouritesEntity = await _favouritesRepository.GetAllByBuyerId(buyerId);

            if (favouritesEntity.Count == 0)
                return new List<Favourite>();

            var favourites = _mapper.Map<List<FavoritesEntity>, List<Favourite>>(favouritesEntity);
            return favourites;
        }
        public bool TryGetFavoriteProduct(HttpContext httpContext, out List<Favourite>? favourites)
        {
            favourites = _sessionWorker.GetEntitiesByKey<Favourite>(httpContext, SESSION_KEY);

            return favourites is null || favourites.Count == 0 ? false : true;
        }
        public bool NonExistFavoriteProduct(HttpContext httpContext, ProductFavoritsPostDto favouritsPostDto)
        {
            var favourites = _sessionWorker.GetEntitiesByKey<Favourite>(httpContext, SESSION_KEY);

            bool hasFavouriteProduct = favourites
                                            .Where(w => w.ProductId == favouritsPostDto.ProductId)
                                            .IsNullOrEmpty();
            return hasFavouriteProduct;
        }
        public OperationStatusResponseBase AddFavoriteProductInSession(HttpContext httpContext, ProductFavoritsPostDto favouritsPostDto)
        {
            var favouriteProducts = _sessionWorker.GetEntitiesByKey<Favourite>(httpContext, SESSION_KEY);

            var favouriteProduct = _mapper.Map<ProductFavoritsPostDto, Favourite>(favouritsPostDto);

            favouriteProducts!.Add(favouriteProduct);

            var result = _sessionWorker.BindEntitiesInSession<Favourite>(httpContext, favouriteProducts, SESSION_KEY);
            return result;
        }
        public async Task<OperationStatusResponseBase> AddFavoriteProductInDatabase(HttpContext httpContext, ProductFavoritsPostDto favouritsPostDto)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);

            var favoriteEntity = new FavoritesEntity 
            {
                BuyerId = buyerId,
                ProductId = favouritsPostDto.ProductId,
            }; 

            var result = await _favouritesRepository.Create(favoriteEntity);
            return result;
        }
        public OperationStatusResponseBase RemoveFavoriteProductFromSession(HttpContext httpContext, int productId)
        {
            var favourites = _sessionWorker.GetEntitiesByKey<Favourite>(httpContext, SESSION_KEY);
            var favouritesForRemoving = favourites!
                                        .Where(w => w.ProductId == productId)
                                        .FirstOrDefault();

            if (favourites!.Remove(favouritesForRemoving!))
            {
                var result = _sessionWorker.BindEntitiesInSession(httpContext, favourites, SESSION_KEY);
                return result;
            }

            return OperationStatusResonceBuilder.CreateStatusWarning("The product could not be deleted from favorites");
        }
        public async Task<OperationStatusResponseBase> RemoveFavoriteProductFromDatabase(HttpContext httpContext, int productId)
        {
            var buyerClaims = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var buyerId = int.Parse(buyerClaims!.Value);

            var favoriteEntity = new FavoritesEntity
            {
                BuyerId = buyerId,
                ProductId = productId,
            };

            var result = await _favouritesRepository.Delete(favoriteEntity);
            return result;
        }       
    }
}
