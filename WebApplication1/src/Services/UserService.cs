using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qoden.Validation;
using WebApplication1.Database;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Repositories;
using WebApplication1.Requests;
using WebApplication1.Responses;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        Task CreateUser(CreateUserRequest req);
        Task AssignManager(AssignManagerRequest req);
        Task UnAssignManager(AssignManagerRequest req);
        Task ModifyUser(int userId, ModifyUserRequest req);
        Task<ProfileResponse> GetProfile(int userId);
        Task<UserInfoResponse> GetUserInfo(int userId);
    }


    public class UserService : IUserService
    {
        private readonly IDbConnectionFactory _db;
        private const string UserNotFoundMsg = "User not found";

        public UserService(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task CreateUser(CreateUserRequest req)
        {

            var user = req.CreateUserFromRequest();
            await CheckUsersUniqueFields(user);
            await _db.InsertUser(user);
        }

        public async Task AssignManager(AssignManagerRequest req)
        {
            var user = await _db.GetUserById(req.UserId);
            var manager = await _db.GetUserById(req.UserId);
            var managerRoles = await _db.GetRolesByUserId(req.ManagerId);

            Check.Value(user).NotNull(UserNotFoundMsg);
            Check.Value(manager).NotNull("Manager not found");
            var managerRole = managerRoles.FirstOrDefault(r => r.Name == "manager");
            Check.Value(managerRole).NotNull("User with specified ID is not a manager");

            var userManager = new UserManager(req.UserId, req.ManagerId);
            await _db.InsertUserManager(userManager);
        }

        public async Task UnAssignManager(AssignManagerRequest req)
        {
            var userManager = await _db.GetUserManagerByIds(req.UserId, req.ManagerId);
            Check.Value(userManager).NotNull("Specified relationship could not be found");
            await _db.DeleteUserManager(userManager);
        }

        public async Task ModifyUser(int userId, ModifyUserRequest req)
        {
            req.Validate(new Validator());
            var user = await _db.GetUserByIdIncludeDepartment(userId);
            Check.Value(user).NotNull(UserNotFoundMsg);
            if (req.DepartmentId != null)
            {
                var depWithSuchId = _db.GetDepartmentById(req.DepartmentId.Value);
                Check.Value(user).NotNull("Department with specified ID could not be found");
            }

            user.ModifyUser(req);
            await CheckUsersUniqueFields(user);
            await _db.UpdateUser(user);
        }

        public async Task<ProfileResponse> GetProfile(int userId)
        {
            var user = await _db.GetUserByIdIncludeDepartment(userId);
            Check.Value(user).NotNull(UserNotFoundMsg);
            return new ProfileResponse(user);
        }


        public async Task<UserInfoResponse> GetUserInfo(int userId)
        {
            var user = await _db.GetUserByIdIncludeDepartment(userId);
            Check.Value(user).NotNull(UserNotFoundMsg);
            return new UserInfoResponse(user);
        }

        private async Task CheckUsersUniqueFields(User user)
        {
            var existingUser = await _db.GetUserByEmail(user.Email);
            if (existingUser != null)
                Check.Value(existingUser.Id).EqualsTo(user.Id, "User with such email already exists");
            existingUser = await _db.GetUserByNickname(user.NickName);
            if (existingUser != null)
                Check.Value(existingUser.Id).EqualsTo(user.Id, "User with such nickname already exists");
            if (user.PhoneNumber != null)
            {
                existingUser =
                    await _db.GetUserByPhoneNumber(user.PhoneNumber);
                if (existingUser != null)
                    Check.Value(existingUser.Id).EqualsTo(user.Id, "User with such phone number already exists");
            }
        }
    }
}