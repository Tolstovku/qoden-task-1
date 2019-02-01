using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Route("/api/v1/admin")]
    [MyAuthorize(Role.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //TODO ??? что лучше: просто две стринги или целый класс под запрос?
        [HttpPut("user")]
        public void AssignDepartment([FromBody] AssignDepartmentRequest req)
        {
            _adminService.AssignDepartment(req);
        }

        [HttpGet("requests")]
        public List<SalaryRateRequest> GetSalaryRateRequests()
        {
            return _adminService.GetSalaryRateRequests();
        }

        [HttpPost("user")]
        public void CreateUser([FromBody] User user)
        {
            _adminService.CreateUser(user);
        }
    }
}