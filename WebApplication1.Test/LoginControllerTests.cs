using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class LoginControllerTests : IClassFixture<ApiFixture>
    {
        private ApiFixture Api { get; set; }

        public LoginControllerTests(ApiFixture api)
        {
            Api = api;
        }

        [Fact]
        public async Task UserCanLogin()
        {
            Api.Db.Users.FirstOrDefault(u => true);
            var request = new LoginRequest
            {
                NicknameOrEmail = "User",
                Password = "123"
            };
            var response = await Api.RegularUser.PostAsJsonAsync("api/v1/login", request);

            var dbOption = new DbContextOptionsBuilder<DatabaseContext>()
                .UseNpgsql("User ID=postgres;Password=xna004;Host=localhost;Port=5432;Database=pip;");
            var db = new DatabaseContext(dbOption.Options);
            var users = db.Users.All(u => true);


            response.StatusCode.Should().BeEquivalentTo(200);
        }

        [Fact]
        public async Task UserCanLogout()
        {
            var response = await Api.RegularUser.PostAsync("api/v1/logout", null);
            response.StatusCode.Should().BeEquivalentTo(200);
        }

        [Theory]
        [InlineData("john@mailinator.com", "12345")]
        [InlineData("michael@mailinator.com", "54321")]
        [InlineData("alice@mailinator.com", "13245")]
        public async Task UserWithInvalidCredentialsCannotLogin(string nicknameOrEmail, string password)
        {
            var request = new LoginRequest
            {
                NicknameOrEmail = nicknameOrEmail,
                Password = password
            };

            var response = await Api.RegularUser.PostAsJsonAsync("api/v1/user/requests", request);
            response.StatusCode.Should().BeEquivalentTo(400);
        }
    }
}