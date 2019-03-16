using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using WebApplication1.Database;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Repositories;
using WebApplication1.Requests;
using WebApplication1.Responses;

namespace WebApplication1.Services
{
    public interface ISalaryRateRequestService
    {
        Task<List<UserSalaryRateRequestsResponse>> GetSalaryRateRequestsByUser(int userId);
        Task<List<SalaryRateRequest>> GetSalaryRateRequestsByManager(int managerId);
        Task AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req);
        Task<List<SalaryRateRequest>> GetAllSalaryRateRequests();
        Task CreateSalaryRateRequest(UserCreateSalaryRateRequestRequest req);
    }

    public class SalaryRateRequestService : ISalaryRateRequestService
    {
        private readonly IDbConnectionFactory _db;
        private const string UserNotFoundMsg = "User not found";

        public SalaryRateRequestService(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<List<UserSalaryRateRequestsResponse>> GetSalaryRateRequestsByUser(int userId)
        {
            var salaryRateRequests = await _db.GetSalaryRequestsByUserId(userId);

            var responseSRRsList = new List<UserSalaryRateRequestsResponse>();
            salaryRateRequests.ForEach(srr =>  responseSRRsList.Add(new UserSalaryRateRequestsResponse(srr)));
            return responseSRRsList;
        }

        public async Task<List<SalaryRateRequest>> GetSalaryRateRequestsByManager(int managerId)
        {
            return await _db.GetSalaryRequestsByManagerId(managerId);

        }

        public async Task AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req)
        {
            req.Validate(new Validator());
            var requestsInChain = await _db.GetSalaryRequestsByChainIdNewestFirst(req.RequestChainId);
            Check.Value(requestsInChain.Count).NotEqualsTo(0, "Request chain with specified ID does not exist");

            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            var previousSRRInChain = requestsInChain.First();
            salaryRateRequest.SuggestedRate = previousSRRInChain.SuggestedRate;
            salaryRateRequest.SenderId = previousSRRInChain.SenderId;
            salaryRateRequest.Reason = previousSRRInChain.Reason;
            salaryRateRequest.SalaryRateRequestStatus = previousSRRInChain.SalaryRateRequestStatus;
            await _db.InsertSalaryRequest(salaryRateRequest);
        }

        public async Task<List<SalaryRateRequest>> GetAllSalaryRateRequests()
        {
            return await _db.GetAllSalaryRequests();
        }

        public async Task CreateSalaryRateRequest(UserCreateSalaryRateRequestRequest req)
        {
            req.Validate(new Validator());
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            await _db.InsertSalaryRequest(salaryRateRequest);
        }
    }
}