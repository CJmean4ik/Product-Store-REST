using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using UsersRestApi.Database.Entities;
using UsersRestApi.DTO;
using UsersRestApi.Entities;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.EmaiAuthService;
using UsersRestApi.Services.Password;

namespace UsersRestApi.Services.UserService
{
    public class UsersService
    {
        private IUserRepository<UserEntity, OperationStatusResponse> _repository;
        private IPasswordHasher<UserEntity> _passwordHasher;
        private IEmailVerifySender _emailSender;
        private IMemoryCache _memoryCache;
        private IMapper _mapper;

        public UsersService(IUserRepository<UserEntity, OperationStatusResponse> repository,
                               IPasswordHasher<UserEntity> passwordHasher,
                               IEmailVerifySender emailSender,
                               IMemoryCache memoryCache,
                               IMapper mapper)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
            _mapper = mapper;
        }

        public async Task<OperationStatusResponse> LoginUser(UserPostDto userForLogin, HttpContext httpContext)
        {
            try
            {
                var userEntity = await _repository.GetByName(userForLogin.Username);

                var user = _mapper.Map<UserEntity, User>(userEntity);

                if (!_passwordHasher.Decryption(userForLogin.EnteredPassword, user.Salt, user.HashPassword))
                    return OperationStatusResonceBuilder.CreateCustomStatus("Invalid password. Entry is not possible", StatusName.Warning);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return OperationStatusResonceBuilder.CreateCustomStatus("The user is authorized", StatusName.Successfully);
            }
            catch (Exception ex)
            {
               return OperationStatusResonceBuilder.CreateCustomStatus(ex.Message, StatusName.Error);
            }
        }
        public OperationStatusResponse RegisterUser(UserPostDto userForRegistering)
        {
            _emailSender.GenerateCode();
            _emailSender.SendCode(userForRegistering.Email);

            _memoryCache.Set("CurrentUser", userForRegistering);
            _memoryCache.Set("CurrentCode", _emailSender.Code);

            return OperationStatusResonceBuilder.CreateCustomStatus("Mail code has been sended", StatusName.SendedMailCode);
        }
        public bool IsVerifyMail(string code)
        {
            string verifyCode = _memoryCache.Get("CurrentCode")!.ToString()!;

            if (!_emailSender.VerifyCode(code, verifyCode))
            {
                _memoryCache.Remove("CurrentCode");
                return false;
            }

            _memoryCache.Remove("CurrentCode");
            return true;
        }
        public async Task<OperationStatusResponse> RegisterUser()
        {
            if (!_memoryCache.TryGetValue("CurrentUser", out UserPostDto? userDto))
                return OperationStatusResonceBuilder.CreateCustomStatus("User data was unexpectedly lost", StatusName.Warning);

            var userEntity = _mapper.Map<UserPostDto, UserEntity>(userDto);

            _passwordHasher.Encryption(userDto!.EnteredPassword, userEntity);

            var result = await _repository.Create(userEntity);

            _memoryCache.Remove("CurrentUser");

            return OperationStatusResonceBuilder.CreateCustomStatus("User Successful aded", StatusName.Created);
        }
    }
}
