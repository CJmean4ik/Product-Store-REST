using AutoMapper;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Carts;
using ProductAPI.DTO.Favourite;
using ProductAPI.DTO.Image;
using ProductAPI.DTO.Orders;
using ProductAPI.DTO.Product;
using ProductAPI.DTO.User;
using ProductAPI.Models;
using UsersRestApi.Database.Entities;
using UsersRestApi.Entities;
using UsersRestApi.Models;

namespace UsersRestApi.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            ProductMapping();

            ImageMapping();

            UsersMapping();

            CartMapping();

            FavouritesMapping();

            OrdersMapping();
        }

        private void OrdersMapping()
        {
            CreateMap<OrderEntity, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(dest => dest.DeliveryType, opt => opt.MapFrom(src => src.DeliveryType))
                .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => new Buyer
                {
                    Name = src.Buyer.Name,
                    Username = src.Buyer.Username,
                    LastName = src.Buyer.LastName!,
                    PhoneNumber = src.Buyer.PhoneNumber,
                    Email = src.Buyer.Email,
                    Surname = src.Buyer.Surname,
                }))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                src.OrderProducts.Select(op => new Product
                {
                    ProductId = op.Product.ProductId,
                    Name = op.Product.Name,
                    Price = op.Product.Price,
                    Description = op.Product.Description,                                             
                }).ToList()));

            CreateMap<OrderPostDto, OrderEntity>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(dest => dest.DeliveryType, opt => opt.MapFrom(src => src.DeliveryType))
                .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => "New"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));

            CreateMap<OrderPostDto, BuyerEntity>()
                  .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                  .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                  .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => "buyer"));
        }
        private void ProductMapping()
        {
            CreateMap<ProductEntity, Product>()
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category
               {
                   CategoryName = src.SubCategory.Category.Name,
                   SubCategoryName = src.SubCategory.Name
               }))
               .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Image
               {
                   PreviewImage = src.PreviewImageName,
                   Images = src.Images.Select(s => s.ImageName).ToList()
               }));


            CreateMap<ProductPostDto, ProductEntity>()
               .ForMember(dest => dest.ProductId, opt => opt.Ignore())
               .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => new SubCategoryEntity { Name = src.SubCategory }))
               .ForMember(dest => dest.PreviewImageName, opt => opt.MapFrom(src => src.PreviewImage.FileName))
               .ForMember(dest => dest.Images, opt => opt.Ignore())
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<ProductPostDto, ImagePostDto>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.Preview, opt => opt.MapFrom(src => src.PreviewImage))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));


            CreateMap<ProductPutDto, ProductEntity>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.TransportId))
                .ForMember(dest => dest.PreviewImageName, opt => opt.MapFrom(src => src.PreviewImage))
               .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => new SubCategoryEntity { Name = src.SubCategory }));

            CreateMap<ProductDelDto, ProductEntity>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.TransportId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
        private void ImageMapping()
        {
            CreateMap<ImagePutDto, ProductEntity>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => new List<ImageEntity> { new ImageEntity { ImageName = src.NewImage.FileName } }));
        }
        private void UsersMapping()
        {
            CreateMap<EmployeeRegistrationPostDto, EmployeeEntity>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));


            CreateMap<BuyerRegistrationPostDto, BuyerEntity>()
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
               .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
               .ForMember(dest => dest.Salt, opt => opt.Ignore())
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


            CreateMap<UserAuthorizePostDto, Employee>();
            CreateMap<EmployeeEntity, Employee>();
            CreateMap<UserAuthorizePostDto, EmployeeEntity>();
        }
        private void CartMapping()
        {
            CreateMap<ProductCartsPostDto, Cart>()
               .ForMember(dest => dest.CartId, opt => opt.MapFrom(_ => new Random().Next(0, 1000)))
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
               .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.InStock))
               .ForMember(dest => dest.PreviewName, opt => opt.MapFrom(src => src.PreviewName));

            CreateMap<CartEntity, ProductCartsPostDto>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
              .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.Product.CountOnStorage == 0 ? false : true))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
              .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
              .ForMember(dest => dest.PreviewName, opt => opt.MapFrom(src => src.Product.PreviewImageName));


            CreateMap<CartEntity, Cart>()
               .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
               .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
               .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.Product.CountOnStorage == 0 ? false : true))
               .ForMember(dest => dest.PreviewName, opt => opt.MapFrom(src => src.Product.PreviewImageName));
        }
        private void FavouritesMapping()
        {
            CreateMap<FavoritesEntity, Favourite>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FavouriteId))
             .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
             .ForMember(dest => dest.PreviewName, opt => opt.MapFrom(src => src.Product.PreviewImageName));

            CreateMap<ProductFavoritsPostDto, Favourite>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => new Random().Next(0, 1000)))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.PreviewName, opt => opt.MapFrom(src => src.PreviewName));
        }
    }
}
