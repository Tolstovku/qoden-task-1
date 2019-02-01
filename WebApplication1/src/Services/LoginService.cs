using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface ILoginService
    {
        ClaimsPrincipal Login(string nickname, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly DatabaseContext _db;

        public LoginService(DatabaseContext db)
        {
            _db = db;
        }

        public ClaimsPrincipal Login(string nickname, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.NickName == nickname && u.Password == password);
            if (user == null) throw new AuthenticationException("There is no such user");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NickName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}