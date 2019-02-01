using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/manager")]
    [MyAuthorize(Role.Manager | Role.Admin)]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("user/{userId}")]
        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return _managerService.GetUserInfo(userId);
        }

        [HttpPost("user")]
        public void ModifyUser([FromBody] User user)
        {
            _managerService.ModifyUser(user);
        }

        [HttpGet("requests/{managerId}")]
        public List<SalaryRateRequest> GetSalaryRateRequests(int managerId)
        {
            return _managerService.GetRateRequests(managerId);
        }

        [HttpPut("requests")]
        public void AnswerSalaryRateRequest([FromBody] AnswerSalaryRateRequestRequest req)
        {
            _managerService.AnswerSalaryRateRequest(req);
        }
    }
}