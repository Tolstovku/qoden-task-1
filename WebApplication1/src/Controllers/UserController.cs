using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
        public ProfileResponse GetProfile(int userId)
        {
            return _userService.GetProfile(userId);
        }

        [HttpGet("profile/{userId}")]
        [Authorize(Roles = "manager, admin")]
        public UserInfoResponse GetUserInfo(int userId)
        {
            return _userService.GetUserInfo(userId);
        }

        [HttpPut("modify")]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> ModifyUser([FromBody] User user)
        {
            try
            {
                _userService.ModifyUser(user);
            }
            catch (DuplicateNameException e)
            {
                Response.StatusCode = 409;
                return Json(e.Message);
            }
            return Ok();
        }
        
        [HttpPut("/assign")]
        [Authorize(Roles = "admin")]
        public void AssignManager([FromBody] AssignManagerRequest req)
        {
            _userService.AssignManager(req);
        }
        
        [HttpPut("/unassign")]
        [Authorize(Roles = "admin")]
        public void UnAssignManager([FromBody] AssignManagerRequest req)
        {
            _userService.UnAssignManager(req);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            try
            {
            _userService.CreateUser(user);
            }
            catch (DuplicateNameException e)
            {
                Response.StatusCode = 409;
                return Json(e.Message);
            }
            return Ok();
        }
        
    }
}
