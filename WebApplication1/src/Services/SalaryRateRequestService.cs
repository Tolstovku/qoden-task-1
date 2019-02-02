using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface ISalaryRateRequestService
    {
        List<GetSalaryRateRequestsByUserResponse> GetSalaryRateRequests(int userId);
        List<SalaryRateRequest> GetRateRequests(int managerId);
        void AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req);
        List<SalaryRateRequest> GetSalaryRateRequests();
        void CreateSalaryRateRequest(CreateSalaryRateRequestByUserRequest req);
    }
    
    public class SalaryRateRequestService : ISalaryRateRequestService
    {
        private DatabaseContext _db;

        public SalaryRateRequestService(DatabaseContext db)
        {
            _db = db;
        }

        public List<GetSalaryRateRequestsByUserResponse> GetSalaryRateRequests(int userId)
        {
            var salaryRateRequests =  _db.SalaryRateRequests
                .Where(srr => srr.SenderId == userId)
                .ToList();
            var responseSRRsList = new List<GetSalaryRateRequestsByUserResponse>();
            salaryRateRequests.ForEach(srr =>  responseSRRsList.Add(new GetSalaryRateRequestsByUserResponse(srr)));
            return responseSRRsList;
        }
        
        public List<SalaryRateRequest> GetRateRequests(int managerId)
        {
            var manager = _db.Users.Include(u => u.Department).SingleOrDefault(u => u.Id == managerId);
            var managerDepartment = manager.Department;
            return _db.SalaryRateRequests
                .Where(srr => srr.Sender.Department == managerDepartment)
                .ToList();
        }

        public void AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req)
        {
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            var previousSRRInChain = _db.SalaryRateRequests
                .Include(srr => srr.Sender)
                .Include(srr => srr.Reviewer)
                .LastOrDefault(srr => srr.RequestChainId == salaryRateRequest.RequestChainId);
            salaryRateRequest.SuggestedRate = previousSRRInChain.SuggestedRate;
            salaryRateRequest.SenderId = previousSRRInChain.SenderId;
            salaryRateRequest.Reason = previousSRRInChain.Reason;
            salaryRateRequest.Status = previousSRRInChain.Status;
            _db.SalaryRateRequests.Add(salaryRateRequest);
            _db.SaveChanges();
        }
        
        public List<SalaryRateRequest> GetSalaryRateRequests()
        {
            return _db.SalaryRateRequests
                .ToList();
        }
        
        public void CreateSalaryRateRequest(CreateSalaryRateRequestByUserRequest req)
        {
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            _db.SalaryRateRequests.Add(salaryRateRequest);
            _db.SaveChanges();
        }
    }
}