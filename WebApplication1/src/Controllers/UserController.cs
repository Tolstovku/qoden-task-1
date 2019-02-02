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
        private readonly ISalaryRateRequestService _salaryRateRequestService;

        public UserController(IUserService userService, ISalaryRateRequestService salaryRateRequestService)
        {
            _userService = userService;
            _salaryRateRequestService = salaryRateRequestService;
        }

        [HttpGet("{userId}")]
        public GetProfileResponse GetProfile(int userId)
        {
            return _userService.GetProfile(userId);
        }

        [HttpPut("requests")]
        public void CreateSalaryRateRequests([FromBody] CreateSalaryRateRequestByUserRequest req)
        {
            _salaryRateRequestService.CreateSalaryRateRequest(req);
        }

        [HttpGet("requests/{userId}")]
        public List<GetSalaryRateRequestsByUserResponse> GetSalaryRateRequests(int userId)
        {
            return _salaryRateRequestService.GetSalaryRateRequestsByUser(userId);
        }
    }
}
