using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IUserService
    {
        GetProfileResponse GetProfile(int userId);
        void CreateSalaryRateRequest(CreateSalaryRateRequestByUserRequest req);
        List<SalaryRateRequest> GetSalaryRateRequests(int userId);
    }

    internal class UserService : IUserService
    {
        private readonly DatabaseContext _db;

        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public GetProfileResponse GetProfile(int userId)
        {
            var user = _db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId);
            return new GetProfileResponse(user);
        }

        public void CreateSalaryRateRequest(CreateSalaryRateRequestByUserRequest req)
        {
            var salaryRateRequest = req.ConvertToSalaryRateRequest();
            _db.SalaryRateRequests.Add(salaryRateRequest);
            _db.SaveChanges();
            //TODO ??? нужно ли возвращать то что создаешь
        }

        public List<SalaryRateRequest> GetSalaryRateRequests(int userId)
        {
            return _db.SalaryRateRequests
                .Where(srr => srr.SenderId == userId)
                .ToList();
        }
    }
}