using AutoMapper;
using UsersRestApi.Database.Entities;
using UsersRestApi.DTO;
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
               .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => new SubCategoryEntity { Name = src.SubCategory }))
               .ForMember(dest => dest.PreviewImageName, opt => opt.Ignore())
               .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ProductPutDto, ProductEntity>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.TransportId))
               .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => new SubCategoryEntity { Name = src.SubCategory }));

            CreateMap<ProductDelDto, ProductEntity>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.TransportId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<UserPostDto, User>();
            CreateMap<UserEntity, User>();
            CreateMap<UserPostDto, UserEntity>();
        }

    }
}
