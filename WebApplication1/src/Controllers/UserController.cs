using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities;
using WebApplication1.Requests;
using WebApplication1.Responses;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("/api/v1/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<ProfileResponse> GetProfile(int userId)
        {
            return await _userService.GetProfile(userId);
        }

        [HttpGet("profile/{userId}")]
        [Authorize(Roles = "manager, admin")]
        public async Task<UserInfoResponse> GetUserInfo(int userId)
        {
            return await _userService.GetUserInfo(userId);
        }

        [HttpPut("modify")]
        [Authorize(Roles = "manager, admin")]
        public async Task ModifyUser([FromBody] User user)
        {
                await _userService.ModifyUser(user);
        }
        
        [HttpPost("assign")]
        [Authorize(Roles = "admin")]
        public async Task AssignManager([FromBody] AssignManagerRequest req)
        {
            await _userService.AssignManager(req);
        }
        
        [HttpPost("unassign")]
        [Authorize(Roles = "admin")]
        public async Task UnAssignManager([FromBody] AssignManagerRequest req)
        {
            await _userService.UnAssignManager(req);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task CreateUser([FromBody] User user)
        {
            await _userService.CreateUser(user);
        }
        
    }
}
