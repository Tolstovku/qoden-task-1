using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface ISalaryRateRequestService
    {
        List<UserSalaryRateRequestsResponse> GetSalaryRateRequestsByUser(int userId);
        List<SalaryRateRequest> GetSalaryRateRequestsByManager(int managerId);
        void AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req);
        List<SalaryRateRequest> GetAllSalaryRateRequests();
        void CreateSalaryRateRequest(UserCreateSalaryRateRequestRequest req);
    }
    
    public class SalaryRateRequestService : ISalaryRateRequestService
    {
        private DatabaseContext _db;

        public SalaryRateRequestService(DatabaseContext db)
        {
            
            _db = db;
        }

        public List<UserSalaryRateRequestsResponse> GetSalaryRateRequestsByUser(int userId)
        {
            var salaryRateRequests =  _db.SalaryRateRequests
                .Where(srr => srr.SenderId == userId)
                .ToList();
            var responseSRRsList = new List<UserSalaryRateRequestsResponse>();
            salaryRateRequests.ForEach(srr =>  responseSRRsList.Add(new UserSalaryRateRequestsResponse(srr)));
            return responseSRRsList;
        }
        
        public List<SalaryRateRequest> GetSalaryRateRequestsByManager(int managerId)
        {
            var myUsersIds = _db.UserManagers.Where(um => um.ManagerId == managerId).Select(um => um.UserId).ToList();
            return _db.SalaryRateRequests.Where(srr => myUsersIds.Contains(srr.SenderId)).ToList();
            
        }

        public void AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req)
        {
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            var previousSRRInChain = _db.SalaryRateRequests
                .Include(srr => srr.Sender)
                .Include(srr => srr.Reviewer)
                .Where(srr => srr.RequestChainId == salaryRateRequest.RequestChainId)
                .OrderByDescending(srr => srr.CreatedAt)
                .FirstOrDefault();
            salaryRateRequest.SuggestedRate = previousSRRInChain.SuggestedRate;
            salaryRateRequest.SenderId = previousSRRInChain.SenderId;
            salaryRateRequest.Reason = previousSRRInChain.Reason;
            salaryRateRequest.SalaryRateRequestStatus = previousSRRInChain.SalaryRateRequestStatus;
            _db.SalaryRateRequests.Add(salaryRateRequest);
            _db.SaveChangesAsync();
        }
        
        public List<SalaryRateRequest> GetAllSalaryRateRequests()
        {
            return _db.SalaryRateRequests
                .ToList();
        }
        
        public void CreateSalaryRateRequest(UserCreateSalaryRateRequestRequest req)
        {
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            _db.SalaryRateRequests.Add(salaryRateRequest);
            _db.SaveChangesAsync();
        }
    }
}