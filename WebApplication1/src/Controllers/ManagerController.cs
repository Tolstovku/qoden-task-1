using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/manager")]
    [Authorize(Roles = "manager,admin")]
    public class ManagerController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISalaryRateRequestService _salaryRateRequestService;

        public ManagerController(IUserService userService, ISalaryRateRequestService salaryRateRequestService)
        {
            _userService = userService;
            _salaryRateRequestService = salaryRateRequestService;
        }

        [HttpGet("user/{userId}")]
        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return _userService.GetUserInfo(userId);
        }

        [HttpPost("user")]
        public void ModifyUser([FromBody] User user)
        {
            _userService.ModifyUser(user);
        }

        [HttpGet("requests/{managerId}")]
        public List<SalaryRateRequest> GetSalaryRateRequests(int managerId)
        {
            return _salaryRateRequestService.GetSalaryRateRequestsByManager(managerId);
        }

        [HttpPut("requests")]
        public void AnswerSalaryRateRequest([FromBody] AnswerSalaryRateRequestRequest req)
        {
            _salaryRateRequestService.AnswerSalaryRateRequest(req);
        }
    }
}