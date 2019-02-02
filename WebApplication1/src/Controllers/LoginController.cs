using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async void Login([FromBody] LoginRequest req)
        {
            var claimsPrincipal = _loginService.Login(req);
            await HttpContext.SignInAsync(claimsPrincipal);
        }


        [HttpPost("logout")]
        public async void Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}