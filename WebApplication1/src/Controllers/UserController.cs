using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/user")]
    [MyAuthorize(Role.User | Role.Admin)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public void GetProfile(int userId)
        {
            _userService.GetProfile(userId);
        }

        [HttpPut("requests")]
        public void CreateSalaryRateRequests([FromBody] CreateSalaryRateRequestByUserRequest req)
        {
            _userService.CreateSalaryRateRequest(req);
        }

        //TODO ??? Получать айди из запроса или из куки?
        [HttpGet("requests/{userId}")]
        public void GetSalaryRateRequests(int userId)
        {
            _userService.GetSalaryRateRequests(userId);
        }
    }
}