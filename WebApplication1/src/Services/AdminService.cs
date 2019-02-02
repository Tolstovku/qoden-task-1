using System.Collections.Generic;
using System.Linq;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IAdminService
    {
        void CreateUser(User user);
        void AssignManager(AssignManagerRequest req);
    }


    public class AdminService : IAdminService
    {
        private readonly DatabaseContext _db;

        public AdminService(DatabaseContext db)
        {
            _db = db;
        }

        public void CreateUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void AssignManager(AssignManagerRequest req)
        {
            var userManager = new UserManager(req.UserId, req.ManagerId);
            _db.UserManagers.Add(userManager);
            _db.SaveChanges();
        }
        
        public void UnAssignManager(AssignManagerRequest req)
        {
            var userManager = new UserManager(req.UserId, req.ManagerId);
            _db.UserManagers.Remove(userManager);
            _db.SaveChanges();
        }
    }
}
