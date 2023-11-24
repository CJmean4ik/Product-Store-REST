using Microsoft.AspNetCore.Mvc;
using UsersRestApi.DTO;
using UsersRestApi.Repositories.OperationStatus;
using UsersRestApi.Services.UserService;

namespace UsersRestApi.Controllers
{
    public class UsersController : Controller
    {
        private UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpPost("/sign-in")]

        public async Task<ActionResult<OperationStatusResponseBase>> SignIn([FromBody] UserPostDto user)
        {
            var result = await _usersService.LoginUser(user, HttpContext);
            return Json(new
            {
                Body = result,
                Code = 401
            });
        }

        [HttpPost("/sign-up")]
        public ActionResult<OperationStatusResponseBase> SignUp(UserPostDto userForRegistering)
        {
            var result = _usersService.SendMailVerifyCode(userForRegistering);
            HttpContext.Response.Cookies.Append("NowIsVerifyMail", "true");

            return Json(new { Body = result, Code = 200 });
        }

        [HttpPost("/verify-mail")]
        public IActionResult VerifyMail([FromForm] string code)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("NowIsVerifyMail"))
                return Json("This endpoint is available to users who are undergoing mail verification");

            if (!_usersService.IsVerifyMail(code))
            {
                HttpContext.Response.Cookies.Delete("NowIsVerifyMail");
                return Json("Wrong code. Enter again");
            }

            HttpContext.Response.Cookies.Append("IsMailVerify", "true");
            return Redirect("/create-account");
        }

        [HttpPost("/create-account")]
        public async Task<IActionResult> CreateAccount()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("IsMailVerify"))
                return Json("This endpoint is available to users who are undergoing mail verification");

            var result = await _usersService.RegisterUser();

            HttpContext.Response.Cookies.Delete("IsMailVerify");
            HttpContext.Response.Cookies.Delete("NowIsVerifyMail");

            return Json(result);
        }

    }
}
