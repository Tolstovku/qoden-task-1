using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
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
            var user = _db.Users.Include(u => u.UserRoles).ThenInclude(userRole => userRole.Role).FirstOrDefault(u => u.NickName == nickname && u.Password == password);
            if (user == null) throw new AuthenticationException("There is no such user");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NickName)
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                
            }
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}