using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISalaryRateRequestService _salaryRateRequestService;

        public AdminController(IUserService userService, ISalaryRateRequestService salaryRateRequestService)
        {
            _userService = userService;
            _salaryRateRequestService = salaryRateRequestService;
        }

        [HttpPut("user/{userId}")]
        public void AssignDepartment([FromBody] AssignManagerRequest req)
        {
            _userService.AssignManager(req);
        }

        [HttpGet("requests")]
        public List<SalaryRateRequest> GetSalaryRateRequests()
        {
            return _salaryRateRequestService.GetSalaryRateRequests();
        }

        [HttpPost("user")]
        public void CreateUser([FromBody] User user)
        {
            _userService.CreateUser(user);
        }
    }
}


// Depatment is not like backend frontend but like developers cleaners engineers etc
// remake managers 