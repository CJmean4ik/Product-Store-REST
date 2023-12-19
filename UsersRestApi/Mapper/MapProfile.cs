using AutoMapper;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Image;
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

            CreateMap<ImagePutDto, ProductEntity>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => new List<ImageEntity> { new ImageEntity { ImageName = src.NewImage.FileName } }));


            CreateMap<EmployeeRegistrationPostDto, EmployeeEntity>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));


            CreateMap<BuyerRegistrationPostDto, BuyerEntity>()
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
               .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
               .ForMember(dest => dest.Salt, opt => opt.Ignore())
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


            CreateMap<UserAuthorizePostDto, Employee>();
            CreateMap<EmployeeEntity, Employee>();
            //CreateMap<BuyerEntity, Buyer>();

            CreateMap<UserAuthorizePostDto, EmployeeEntity>();
        }

    }
}
