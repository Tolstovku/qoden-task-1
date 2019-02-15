using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Lesson1.Helpers;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using Qoden.Validation.AspNetCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task AssignManager(AssignManagerRequest req);
        Task UnAssignManager(AssignManagerRequest req);
        Task ModifyUser(User user);
        Task<ProfileResponse> GetProfile(int userId);
        Task<UserInfoResponse> GetUserInfo(int userId);
    }
    
    
    public class UserService : IUserService
    {
        private readonly DatabaseContext _db;
        private const string UserNotFoundMsg = "User not found";

        public UserService(DatabaseContext db)
        {
            _db = db;
        }
        
        public async Task CreateUser(User user)
        {
            user.Validate(new Validator());
            CheckUsersUniqueFields(user);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task AssignManager(AssignManagerRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == req.UserId);
            var manager = await _db.Users.Include(u => u.UserRoles).ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(u => u.Id == req.ManagerId);
            Assert.Property(user).NotNull(UserNotFoundMsg);
            Assert.Property(manager).NotNull("Manager not found");
            var managerRole = manager.UserRoles.FirstOrDefault(ur => ur.Role.Name == "Manager");
            Assert.Property(managerRole).NotNull("User with specified ID is not a manager");
            
            var userManager = new UserManager(req.UserId, req.ManagerId);
            _db.UserManagers.Add(userManager);
            await _db.SaveChangesAsync();
        }
        
        public async Task UnAssignManager(AssignManagerRequest req)
        {
            var userManager = new UserManager(req.UserId, req.ManagerId);
            Assert.Property(userManager).NotNull("Specified relationship could not be found");
            _db.UserManagers.Remove(userManager);
            await _db.SaveChangesAsync();
        }
        
        public async Task ModifyUser(User user)
        {
            Assert.Property(user).NotNull(UserNotFoundMsg);
            user.Validate(new Validator());
            CheckUsersUniqueFields(user);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
        
        public async Task<ProfileResponse> GetProfile(int userId)
        {
            var user = await _db.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == userId);
            return new ProfileResponse(user);
        }
        

        public async Task<UserInfoResponse> GetUserInfo(int userId)
        {
            var user = await _db.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == userId);
            Assert.Property(user).NotNull(UserNotFoundMsg);
            return new UserInfoResponse(user);
        }
        
        private async Task CheckUsersUniqueFields(User user)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            Assert.Property(existingUser).IsNull("User with such email already exists");
            existingUser = await _db.Users.FirstOrDefaultAsync(u => u.NickName == user.NickName);
            Assert.Property(existingUser).IsNull("User with such nickname already exists");
            existingUser = await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
            Assert.Property(existingUser).IsNull("User with such phone number already exists");
        }
    }
}