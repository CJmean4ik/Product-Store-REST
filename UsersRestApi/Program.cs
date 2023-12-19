#region Namespaces

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Image;
using ProductAPI.Repositories.Implementers;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.CartService;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.Implementers;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.EmaiAuthService;
using UsersRestApi.Services.ImageService;
using UsersRestApi.Services.Password;
using UsersRestApi.Services.PasswordHasherService;
using UsersRestApi.Services.ProductService;
using UsersRestApi.Services.UserService;

#endregion

namespace UsersRestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            string CONNECTION_STRING = builder.Configuration.GetConnectionString("DefaultConnection")!;

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddMemoryCache();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(option => 
            {
                option.Cookie.Name = ".AspNetCore.Session.ProductCarts";
                option.IdleTimeout = TimeSpan.FromDays(5);
                option.Cookie.IsEssential = true;
            });


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(option => option.LoginPath = "/sign-in");
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<DatabaseContext>(optionsAction => optionsAction.UseSqlServer(CONNECTION_STRING), ServiceLifetime.Singleton);

            builder.Services.Configure<ImageConfig>(builder.Configuration.GetSection("PathToImages"));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductModifierArgumentChanger, ProductModifierArgumentChanger>();
            builder.Services.AddScoped<IUserRepository<BaseUserEntity, OperationStatusResponseBase>, UserRepository>();
            builder.Services.AddScoped<IPasswordHasher<BaseUserEntity>, PasswordHasher>();
            builder.Services.AddScoped<IEmailVerifySender, EmailVerifySender>();
            builder.Services.AddScoped<IImageReposiroty<IFormFile, OperationStatusResponseBase, ImagePutDto>, ImageRepository>();

            builder.Services.AddScoped<ICartsRepository, CartsRepository>();

            builder.Services.AddScoped<ProductsService>();
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<ImagesService>();

            builder.Services.AddScoped<ProductCartService>();

            var app = builder.Build();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSession();

            app.Run();
        }
    }
}