#region Namespaces

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.EF.UpdateComponents.Order;
using ProductAPI.Database.EF.UpdateComponents.Product;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.Image;
using ProductAPI.Repositories.Implementers;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Repositories.Interfaces.Operations;
using ProductAPI.Services;
using ProductAPI.Services.SessionService;
using UsersRestApi.Database.EF;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.Implementers;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.EmaiAuthService;
using UsersRestApi.Services.Password;
using UsersRestApi.Services.PasswordHasherService;

#endregion

namespace UsersRestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string CONNECTION_STRING = builder.Configuration.GetConnectionString("DefaultConnection")!;

            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMemoryCache();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(option =>
            {
                option.Cookie.Name = ".AspNetCore.Session.Products";
                option.IdleTimeout = TimeSpan.FromDays(10);
                option.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(option => option.LoginPath = "/sign-in");
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<DatabaseContext>(optionsAction => optionsAction.UseSqlServer(CONNECTION_STRING), ServiceLifetime.Singleton);

            builder.Services.Configure<ImageConfig>(builder.Configuration.GetSection("PathToImages"));

            builder.Services.AddScoped<IProductRepository, ProductRepository>()
                            .AddScoped<IProductModifierArgumentChanger, ProductModifierArgumentChanger>()
                            .AddScoped<IOrderModifireArgumentChanger, OrderModifierArgumentChanger>()
                            .AddScoped<IUserRepository<BaseUserEntity, OperationStatusResponseBase>, UserRepository>()
                            .AddScoped<IPasswordHasher<BaseUserEntity>, PasswordHasher>()
                            .AddScoped<IEmailVerifySender, EmailVerifySender>()
                            .AddScoped<IImageReposiroty<IFormFile, OperationStatusResponseBase, ImagePutDto>, ImageRepository>()
                            .AddScoped<ICartsRepository, CartsRepository>()
                            .AddScoped<IFavoritesRepository,FavoritesRepository>()
                            .AddScoped<ISessionWorker<OperationStatusResponseBase>,SessionWorker>()
                            .AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<ProductsService>()
                            .AddScoped<UsersService>()
                            .AddScoped<ImagesService>()
                            .AddScoped<FavoritesService>()
                            .AddScoped<CartService>()
                            .AddScoped<OrderService>();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSession();

            app.UseSwagger()
               .UseSwaggerUI(config => 
               {
                   config.RoutePrefix = string.Empty;
                   config.SwaggerEndpoint("/swagger/v1/swagger.json", "PRODUCT REST API");
               });

         
            app.Run();

        }
    }
}