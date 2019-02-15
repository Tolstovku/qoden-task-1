using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using Qoden.Validation.AspNetCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface ILoginService
    {
        Task<ClaimsPrincipal> Login(LoginRequest request);
    }

    public class LoginService : ILoginService
    {
        private readonly DatabaseContext _db;
        private const string invalidLoginMsg = "Wrong username/email or password";

        public LoginService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<ClaimsPrincipal> Login(LoginRequest req)
        {
            var user = await _db.Users.Include(u => u.UserRoles).ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(u => u.NickName == req.NicknameOrEmail || u.Email == req.NicknameOrEmail);
            Assert.Property(user).NotNull(invalidLoginMsg);
            var verificationResult = PasswordGenerator.VerifyPassword(req.Password, user.Password);
            Assert.Property(verificationResult, "Login")
                .EqualsTo(PasswordVerificationResult.Success, invalidLoginMsg);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                
            }
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}