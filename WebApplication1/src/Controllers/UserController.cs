using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/user")]
    [Authorize(Roles = "user,admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public GetProfileResponse GetProfile(int userId)
        {
            return _userService.GetProfile(userId);
        }

        [HttpGet("profile/{userId}")]
        [Authorize(Roles = "manager, admin")]
        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return _userService.GetUserInfo(userId);
        }

        [HttpPut("modify")]
        [Authorize(Roles = "manager, admin")]
        public void ModifyUser([FromBody] User user)
        {
            _userService.ModifyUser(user);
        }
        
        [HttpPut("{userId}")]
        [Authorize(Roles = "admin")]
        public void AssignManager([FromBody] AssignManagerRequest req)
        {
            _userService.AssignManager(req);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public void CreateUser([FromBody] User user)
        {
            _userService.CreateUser(user);
        }
        
    }
}
