using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTO.User;
using ProductAPI.Services;
using UsersRestApi.Repositories.OperationStatus;

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
        public async Task<ActionResult<OperationStatusResponseBase>> SignIn([FromForm] UserAuthorizePostDto user)
        {
            var result = await _usersService.LoginUser(user, HttpContext);
            return Json(result);
        }

        [HttpPost("/log-out")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationStatusResponseBase>> LogOut()
        {
            var result = await _usersService.LogOutUser(HttpContext);
            return Json(result);
        }


        [HttpPost("/sign-up/employee")]
        [Authorize(Roles = "admin")]
        public ActionResult<OperationStatusResponseBase> SignUp([FromForm] EmployeeRegistrationPostDto userForRegistering)
        {
            var result = registerUserBasedOnRole(userForRegistering);
            return result;
        }

        [HttpPost("/sign-up/buyer")]
        public ActionResult<OperationStatusResponseBase> SignUp([FromForm] BuyerRegistrationPostDto userForRegistering)
        {
            var result = registerUserBasedOnRole(userForRegistering);
            return RedirectToAction("VerifyMail", "UsersController");
        }

        [HttpPost("/verify-mail")]
        public IActionResult VerifyMail([FromForm] string code)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("NowIsVerifyMail"))
                return Json("This endpoint is available to users who are undergoing mail verification");

            if (!_usersService.IsVerifyMail(code))
                return Json("Wrong code. Enter again");


            HttpContext.Response.Cookies.Append("IsMailVerify", "true");
            HttpContext.Response.Cookies.Delete("NowIsVerifyMail");
            return Ok();
        }

        [HttpPost("/create-account")]
        public async Task<IActionResult> CreateAccount()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("IsMailVerify"))
                return Json("This endpoint is available to users who are undergoing mail verification");

            var result = await _usersService.RegisterUser();

            HttpContext.Response.Cookies.Delete("IsMailVerify");

            return RedirectToAction("SignIn", "UsersController");
        }

        private ActionResult<OperationStatusResponseBase> registerUserBasedOnRole(UserBaseDto userBase)
        {
            var result = _usersService.SendMailVerifyCode(userBase);
            HttpContext.Response.Cookies.Append("NowIsVerifyMail", "true");

            return Json(new { Body = result, Code = 200 });
        }
    }
}
