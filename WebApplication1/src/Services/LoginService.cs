using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using WebApplication1.Database;
using WebApplication1.Database.Repositories;
using WebApplication1.Requests;

namespace WebApplication1.Services
{
    public interface ILoginService
    {
        Task<ClaimsPrincipal> Login(LoginRequest request);
    }

    public class LoginService : ILoginService
    {
        private readonly IDbConnectionFactory _db;
        private const string InvalidLoginMsg = "Wrong username/email or password";

        public LoginService(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<ClaimsPrincipal> Login(LoginRequest req)
        {

            var user = await _db.GetUserByEmailOrNickname(req.NicknameOrEmail);
            Check.Value(user).NotNull(InvalidLoginMsg);
            var verificationResult = PasswordGenerator.VerifyPassword(req.Password, user.Password);
            Check.Value(verificationResult, "Login")
                .EqualsTo(PasswordVerificationResult.Success, InvalidLoginMsg);

            var userRoles = await _db.GetRolesByUserId(user.Id);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Name));

            }
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}