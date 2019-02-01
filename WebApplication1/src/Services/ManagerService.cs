using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IManagerService
    {
        void ModifyUser(User user);
        GetUserInfoResponse GetUserInfo(int userId);
        List<SalaryRateRequest> GetRateRequests(int managerId);
        void AnswerSalaryRateRequest(AnswerSalaryRateRequestRequest req);
    }

    public class ManagerService : IManagerService
    {
        private readonly DatabaseContext _db;

        public ManagerService(DatabaseContext db)
        {
            _db = db;
        }

        public void ModifyUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return new GetUserInfoResponse(_db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId));
            _db.Users.Find(userId);
        }

        public List<SalaryRateRequest> GetRateRequests(int managerId)
        {
            var manager = _db.Users.Include(u => u.Department).SingleOrDefault(u => u.Id == managerId);
            var managerDepartment = manager.Department;
            return _db.SalaryRateRequests
                .Where(srr => srr.Sender.Department == managerDepartment)
                .ToList();
        }

        //TODO ??? половина логики тут половина там
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
    }
}