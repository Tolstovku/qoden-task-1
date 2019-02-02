using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IUserService
    {
        void CreateUser(User user);
        void AssignManager(AssignManagerRequest req);
        void UnAssignManager(AssignManagerRequest req);
        void ModifyUser(User user);
        GetProfileResponse GetProfile(int userId);
        GetUserInfoResponse GetUserInfo(int userId);
    }
    
    
    public class UserService : IUserService
    {
        private readonly DatabaseContext _db;

        public UserService(DatabaseContext db)
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
        
        public void ModifyUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }
        
        public GetProfileResponse GetProfile(int userId)
        {
            var user = _db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId);
            return new GetProfileResponse(user);
        }
        

        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return new GetUserInfoResponse(_db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId));
        }
    }
}