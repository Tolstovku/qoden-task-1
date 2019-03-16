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
    [Collection("ApiFixture")]
    public class LoginControllerTests
    {
        private ApiFixture Api { get; set; }
        private ITestOutputHelper Output { get; set; }

        public LoginControllerTests(ApiFixture api, ITestOutputHelper output)
        {
            Api = api;
            Output = output;
        }

        [Fact]
        public async Task UserCanLogin()
        {
            var request = new LoginRequest
            {
                NicknameOrEmail = "User",
                Password = "123"
            };
            var response = await Api.RegularUser.PostAsJsonAsync("api/v1/login", request);
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