using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using ProductAPI.Database.Entities;
using ProductAPI.DTO.User;
using System.Security.Claims;
using UsersRestApi.Database.Entities;
using UsersRestApi.Repositories.Interfaces;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.EmaiAuthService;
using UsersRestApi.Services.Password;

namespace UsersRestApi.Services.UserService
{
    public class UsersService
    {
        private IUserRepository<BaseUserEntity, OperationStatusResponseBase> _repository;
        private IPasswordHasher<BaseUserEntity> _passwordHasher;
        private IEmailVerifySender _emailSender;
        private IMemoryCache _memoryCache;
        private IMapper _mapper;

        public UsersService(IUserRepository<BaseUserEntity, OperationStatusResponseBase> repository,
                               IPasswordHasher<BaseUserEntity> passwordHasher,
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

        public async Task<OperationStatusResponseBase> LoginUser(UserAuthorizePostDto userForLogin, HttpContext httpContext)
        {
            try
            {
                var userEntity = await _repository.GetByName(userForLogin.Username);

                if (userEntity is null) return OperationStatusResonceBuilder.CreateStatusWrongUsername();

                if (!_passwordHasher.Decryption(userForLogin.EnteredPassword, userEntity.Salt, userEntity.HashPassword))
                    return OperationStatusResonceBuilder.CreateStatusWrongPassword();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userEntity.Username),
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim(ClaimTypes.Role, userEntity.Role),
                    new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString())
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

        public async Task<OperationStatusResponseBase> LogOutUser(HttpContext httpContext)
        {
            try
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("User log-out!");
            }
            catch (Exception ex)
            {
                return OperationStatusResonceBuilder.CreateStatusError(ex);
            }
        }

        public OperationStatusResponseBase SendMailVerifyCode(UserBaseDto user)
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
            if (!_memoryCache.TryGetValue("CurrentUser", out UserBaseDto? userDto))
                return OperationStatusResonceBuilder
                    .CreateCustomStatus<object>("User data was unexpectedly lost", StatusName.Warning,null);


            BaseUserEntity userForAdding = new BaseUserEntity();

            if (userDto is EmployeeRegistrationPostDto employee)          
                userForAdding = _mapper.Map<EmployeeRegistrationPostDto, EmployeeEntity>(employee);

            if (userDto is BuyerRegistrationPostDto buyer)
                userForAdding = _mapper.Map<BuyerRegistrationPostDto, BuyerEntity>(buyer);


            _passwordHasher.Encryption(userDto!.EnteredPassword, userForAdding);

            var result = await _repository.Create(userForAdding);

            _memoryCache.Remove("CurrentUser");

            return OperationStatusResonceBuilder.CreateStatusRegistered(message: result.Message);
        }
    }
}
