using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database.Entities;
using WebApplication1.Requests;
using WebApplication1.Responses;
using WebApplication1.Services;

namespace WebApplication1.Controllers
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
        public async Task CreateSalaryRateRequests([FromBody] UserCreateSalaryRateRequestRequest req)
        {
            req.SenderId = this.GetLoggedUserId();
            await _salaryRateRequestService.CreateSalaryRateRequest(req);
        }

        [HttpGet("user/requests")]
        public async Task<List<UserSalaryRateRequestsResponse>> GetSalaryRateRequestsByUser()
        {
            var userId = this.GetLoggedUserId();
            return await _salaryRateRequestService.GetSalaryRateRequestsByUser(userId);
        }
        
        [HttpGet("manager/requests")]
        [Authorize(Roles = "manager, admin")]
        public async Task<List<SalaryRateRequest>> GetSalaryRateRequestsByManager()
        {
            var managerId = this.GetLoggedUserId();
            return await _salaryRateRequestService.GetSalaryRateRequestsByManager(managerId);
        }

        [HttpPost("manager/requests")]
        [Authorize(Roles = "manager, admin")]
        public async Task AnswerSalaryRateRequest([FromBody] AnswerSalaryRateRequestRequest req)
        {
            req.ReviewerId = this.GetLoggedUserId();
            await _salaryRateRequestService.AnswerSalaryRateRequest(req);
        }
        
        
        [HttpGet("admin/requests")]
        [Authorize(Roles = "admin")]
        public async Task<List<SalaryRateRequest>> GetAllSalaryRateRequests()
        {
            return await _salaryRateRequestService.GetAllSalaryRateRequests();
        }
        
    }
}