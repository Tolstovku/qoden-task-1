using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
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
        private readonly DatabaseContext _db;
        private const string userNotFoundMsg = "User not found";
            
        public SalaryRateRequestService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<UserSalaryRateRequestsResponse>> GetSalaryRateRequestsByUser(int userId)
        {
            var user = await _db.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == userId);
            
            var salaryRateRequests =  await _db.SalaryRateRequests
                .Where(srr => srr.SenderId == userId)
                .ToListAsync();
            var responseSRRsList = new List<UserSalaryRateRequestsResponse>();
            salaryRateRequests.ForEach(srr =>  responseSRRsList.Add(new UserSalaryRateRequestsResponse(srr)));
            return responseSRRsList;
        }

        public async Task<List<SalaryRateRequest>> GetSalaryRateRequestsByManager(int managerId)
        {
            var myUsersIds = await _db.UserManagers.Where(um => um.ManagerId == managerId).Select(um => um.UserId).ToListAsync();
            return await _db.SalaryRateRequests.Where(srr => myUsersIds.Contains(srr.SenderId)).ToListAsync();

        }

        public async Task AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req)
        {
            req.Validate(new Validator());
            var chain = await _db.SalaryRateRequests.FirstOrDefaultAsync(srr => srr.RequestChainId == req.RequestChainId);
            Assert.Property(chain).NotNull("Request chain with specified ID does not exist");
            
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            var previousSRRInChain = await _db.SalaryRateRequests
                .Include(srr => srr.Sender)
                .Include(srr => srr.Reviewer)
                .Where(srr => srr.RequestChainId == salaryRateRequest.RequestChainId)
                .OrderByDescending(srr => srr.CreatedAt)
                .FirstOrDefaultAsync();
            salaryRateRequest.SuggestedRate = previousSRRInChain.SuggestedRate;
            salaryRateRequest.SenderId = previousSRRInChain.SenderId;
            salaryRateRequest.Reason = previousSRRInChain.Reason;
            salaryRateRequest.SalaryRateRequestStatus = previousSRRInChain.SalaryRateRequestStatus;
            _db.SalaryRateRequests.Add(salaryRateRequest);
            await _db.SaveChangesAsync();
        }

        public async Task<List<SalaryRateRequest>> GetAllSalaryRateRequests()
        {
            return await _db.SalaryRateRequests
                .ToListAsync();
        }

        public async Task CreateSalaryRateRequest(UserCreateSalaryRateRequestRequest req)
        {
            req.Validate(new Validator());
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            _db.SalaryRateRequests.Add(salaryRateRequest);
            await _db.SaveChangesAsync();
        }
    }
}