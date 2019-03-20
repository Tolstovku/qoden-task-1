using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tests.Fixtures;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Repositories;
using WebApplication1.Requests;
using WebApplication1.Responses;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Collection("ApiFixture")]
    public class SalaryRateRequestControllerTests
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
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfSRRsBefore = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                var response = await Api.RegularUser.PostAsJsonAsync("api/v1/user/requests", srrRequest);
                var amountOfSRRsAfter = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                
                response.StatusCode.Should().BeEquivalentTo(200);
                amountOfSRRsAfter.Should().BeGreaterThan(amountOfSRRsBefore);
            }
        }

        [Fact]
        public async Task UserCanGetHisRequests()
        {
            var response = await Api.RegularUser.GetAsync("api/v1/user/requests");
            var body = await response.Content.ReadAsStringAsync();
            var requests = JsonConvert.DeserializeObject<IEnumerable<UserSalaryRateRequestsResponse>>(body);
            
            response.StatusCode.Should().BeEquivalentTo(200);
            requests.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ManagerCanAnswerRequest()
        {
            var request = new AnswerSalaryRateRequestRequest
            {
                InternalComment = "Test",
                RequestChainId = 1,
                ReviewerComment = "Test",
                SalaryRateRequestStatus = SalaryRateRequestStatus.Fulfilled
            };

            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfSRRsBefore = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                var response = await Api.ManagerUser.PostAsJsonAsync("api/v1/manager/requests", request);
                var amountOfSRRsAfter = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                
                response.StatusCode.Should().BeEquivalentTo(200);
                amountOfSRRsAfter.Should().BeGreaterThan(amountOfSRRsBefore);
            }

        }
        
        [Theory]
        [InlineData(-1, "Test", null)]
        [InlineData(null, "Test", SalaryRateRequestStatus.Fulfilled)]
        [InlineData(-1, null, SalaryRateRequestStatus.Fulfilled)]
        public async Task ManagerCannotMakeInvalidRequestAnswer(int chainId,
            string reviewerComment, SalaryRateRequestStatus status)
             
        {
            var request = new AnswerSalaryRateRequestRequest
            {
                RequestChainId = chainId,
                ReviewerComment = reviewerComment,
                SalaryRateRequestStatus = status
            };

            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfSRRsBefore = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                var response = await Api.ManagerUser.PostAsJsonAsync("api/v1/manager/requests", request);
                var amountOfSRRsAfter = (await Api.ConnectionFactory.GetAllSalaryRequests()).Count();
                
                response.StatusCode.Should().BeEquivalentTo(400);
                amountOfSRRsAfter.Should().Be(amountOfSRRsBefore);
            }
        }

        [Fact]
        public async Task AdminCanGetAllRequests()
        {
            var response = await Api.AdminUser.GetAsync("api/v1/admin/requests");
            var body = await response.Content.ReadAsStringAsync();
            var requests = JsonConvert.DeserializeObject<IEnumerable<SalaryRateRequest>>(body);
            
            response.StatusCode.Should().BeEquivalentTo(200);
            requests.Count().Should().Be(2);
        }
    }
}