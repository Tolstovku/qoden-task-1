using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using WebApplication1.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class SalaryRateRequestControllerTests : IClassFixture<ApiFixture>
    {
        private ApiFixture Api { get; set; }

        public SalaryRateRequestControllerTests(ApiFixture api)
        {
            Api = api;
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

        [Fact]
        public async Task UserCanGetHisRequests()
        {
            var response = await Api.RegularUser.GetAsync("api/v1/user/requests");
                response.StatusCode.Should().BeEquivalentTo(200);
        }
    }
}