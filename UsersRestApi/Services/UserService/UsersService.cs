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
        private IUserRepository<UserEntity, OperationStatusResponseBase> _repository;
        private IPasswordHasher<UserEntity> _passwordHasher;
        private IEmailVerifySender _emailSender;
        private IMemoryCache _memoryCache;
        private IMapper _mapper;

        public UsersService(IUserRepository<UserEntity, OperationStatusResponseBase> repository,
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

        public async Task<OperationStatusResponseBase> LoginUser(UserPostDto userForLogin, HttpContext httpContext)
        {
            try
            {
                var userEntity = await _repository.GetByName(userForLogin.Username);

                if (userEntity is null) return OperationStatusResonceBuilder.CreateStatusWrongUsername();

                var user = _mapper.Map<UserEntity, User>(userEntity);

                if (!_passwordHasher.Decryption(userForLogin.EnteredPassword, user.Salt, user.HashPassword))
                    return OperationStatusResonceBuilder.CreateStatusWrongPassword();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return OperationStatusResonceBuilder.CreateStatusAuthorized();
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public OperationStatusResponseBase SendMailVerifyCode(UserPostDto user)
        {
            try
            {
                _emailSender.GenerateCode();
                _emailSender.SendCode(user.Email);

                _memoryCache.Set("CurrentUser", user);
                _memoryCache.Set("CurrentCode", _emailSender.Code);

                return OperationStatusResonceBuilder.CreateStatusSendedMailCode();
            }
            catch (Exception ex)
            {

                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }
        public bool IsVerifyMail(string code)
        {
            string verifyCode = _memoryCache.Get("CurrentCode")!.ToString()!;

            _memoryCache.Remove("CurrentCode");

            if (!_emailSender.VerifyCode(code, verifyCode))
                return false;

            return true;
        }
        public async Task<OperationStatusResponseBase> RegisterUser()
        {
            if (!_memoryCache.TryGetValue("CurrentUser", out UserPostDto? userDto))
                return OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("User data was unexpectedly lost", StatusName.Warning,null);

            var userEntity = _mapper.Map<UserPostDto, UserEntity>(userDto);

            _passwordHasher.Encryption(userDto!.EnteredPassword, userEntity);

            var result = await _repository.Create(userEntity);

            _memoryCache.Remove("CurrentUser");

            return OperationStatusResonceBuilder.CreateStatusRegistered();
        }
    }
}
