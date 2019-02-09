using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.Database.Entities.Services;

namespace WebApplication1.Database.Entities.Controllers
{
    [Authorize]
    [Route("/api/v1")]
    public class SalaryRateRequestController : Controller
    {
        private readonly ISalaryRateRequestService _salaryRateRequestService;
        
        public SalaryRateRequestController(ISalaryRateRequestService salaryRateRequestService)
        {
            _salaryRateRequestService = salaryRateRequestService;
        }
        
        [HttpPost("user/requests")]
        public void CreateSalaryRateRequests([FromBody] UserCreateSalaryRateRequestRequest req)
        {
            _salaryRateRequestService.CreateSalaryRateRequest(req);
        }

        [HttpGet("user/requests/{userId}")]
        public List<UserSalaryRateRequestsResponse> GetSalaryRateRequestsByUser(int userId)
        {
            return _salaryRateRequestService.GetSalaryRateRequestsByUser(userId);
        }
        
        [HttpGet("manager/requests/{managerId}")]
        [Authorize(Roles = "manager, admin")]
        public List<SalaryRateRequest> GetSalaryRateRequestsByManager(int managerId)
        {
            return _salaryRateRequestService.GetSalaryRateRequestsByManager(managerId);
        }

        [HttpPost("manager/requests")]
        [Authorize(Roles = "manager, admin")]
        public void AnswerSalaryRateRequest([FromBody] AnswerSalaryRateRequestRequest req)
        {
            _salaryRateRequestService.AnswerSalaryRateRequest(req);
        }
        
        
        [HttpGet("/admin/requests")]
        [Authorize(Roles = "admin")]
        public List<SalaryRateRequest> GetAllSalaryRateRequests()
        {
            return _salaryRateRequestService.GetAllSalaryRateRequests();
        }
        
    }
}