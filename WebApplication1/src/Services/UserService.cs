using System.Data;
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
            CheckUsersUniqueFields(user);
            _db.Users.Add(user);
            _db.SaveChangesAsync();
        }

        public void AssignManager(AssignManagerRequest req)
        {
            var userManager = new UserManager(req.UserId, req.ManagerId);
            _db.UserManagers.Add(userManager);
            _db.SaveChangesAsync();
        }
        
        public void UnAssignManager(AssignManagerRequest req)
        {
            var userManager = new UserManager(req.UserId, req.ManagerId);
            _db.UserManagers.Remove(userManager);
            _db.SaveChangesAsync();
        }
        
        public void ModifyUser(User user)
        {
            CheckUsersUniqueFields(user);
            _db.Users.Update(user);
            _db.SaveChangesAsync();
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

        private void CheckUsersUniqueFields(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                _db.SaveChangesAsync();
                throw new DuplicateNameException("User with such email already exists");
            }
            existingUser = _db.Users.FirstOrDefault(u => u.NickName == user.NickName);
            if (existingUser != null)
            {
                _db.SaveChangesAsync();
                throw new DuplicateNameException("User with such nickname already exists");
            }
            existingUser = _db.Users.FirstOrDefault(u => u.PhoneNumber == user.PhoneNumber);
            if (existingUser != null)
            {
                _db.SaveChangesAsync();
                throw new DuplicateNameException("User with such phone number already exists");
            }

        }
    }
}