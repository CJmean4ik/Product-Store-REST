using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents;
using UsersRestApi.Database.Entities;
using UsersRestApi.Models;
using UsersRestApi.Repositories;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.EmaiAuthService;
using UsersRestApi.Services.Password;
using UsersRestApi.Services.PasswordHasherService;
using UsersRestApi.Services.ProductService;
using UsersRestApi.Services.UserService;

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

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(option => option.LoginPath = "/sign-in");
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<DatabaseContext>(optionsAction => optionsAction.UseSqlServer(CONNECTION_STRING), ServiceLifetime.Singleton);

            builder.Services.Configure<ImageConfig>(builder.Configuration.GetSection("PathToImages"));

            builder.Services.AddScoped<IProductRepository, ProductRepositoryEF>();
            builder.Services.AddScoped<IProductModifierArgumentChanger, ProductModifierArgumentChanger>();
            builder.Services.AddScoped<IUserRepository<UserEntity, OperationStatusResponseBase>, UserRepositoryEF>();
            builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher>();
            builder.Services.AddScoped<IEmailVerifySender, EmailVerifySender>();

            builder.Services.AddScoped<ProductsService>();
            builder.Services.AddScoped<UsersService>();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}