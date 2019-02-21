using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using WebApplication1.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class LoginControllerTests : IClassFixture<ApiFixture>
    {
        private ApiFixture Api { get; set; }
        private readonly ITestOutputHelper _testOutputHelper;

        public LoginControllerTests(ApiFixture api, ITestOutputHelper testOutputHelper)
        {
            Api = api;
            _testOutputHelper = testOutputHelper;
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

        [Fact]
        public async Task UserCanCreateSalaryRateRequest()
        {
            var srrRequest = new UserCreateSalaryRateRequestRequest
            {
                SuggestedRate = 123456,
                Reason = "Want money"
            };
            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await Api.RegularUser.PostAsJsonAsync("api/v1/user/requests", srrRequest);
                response.StatusCode.Should().BeEquivalentTo(200);
            }
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