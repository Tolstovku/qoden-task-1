using System.Collections.Generic;
using System.Linq;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IAdminService
    {
        void CreateUser(User user);
        void AssignDepartment(AssignDepartmentRequest req);
        List<SalaryRateRequest> GetSalaryRateRequests();
    }


    //TODO ??? где должны быть асинки в контроллерах онли или в сервисах тоже?
    public class AdminService : IAdminService
    {
        private readonly DatabaseContext _db;

        public AdminService(DatabaseContext db)
        {
            _db = db;
        }

        //ПЕРЕДЕЛАТЬ
        public void CreateUser(User user)
        {
//            user.InvitedAt = DateTimeCreator.getDateTime();
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void AssignDepartment(AssignDepartmentRequest req)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == req.UserId);
            user.DepartmentId = req.DepartmentId;
            _db.SaveChanges();
        }

        public List<SalaryRateRequest> GetSalaryRateRequests()
        {
            return _db.SalaryRateRequests
                .ToList();
        }
    }
}