using System;

namespace WebApplication1.Database.Entities.Requests
{
    public class UserSalaryRateRequestsResponse
    {
        public int SuggestedRate { get; set; }
        public SalaryRateRequestStatus SalaryRateRequestStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ReviewerId { get; set; }
        public int SenderId { get; set; }
        public string ReviewerComment { get; set; }
        public string Reason { get; set; }

        public UserSalaryRateRequestsResponse(SalaryRateRequest srr)
        {
            SuggestedRate = srr.SuggestedRate;
            SalaryRateRequestStatus = srr.SalaryRateRequestStatus;
            CreatedAt = srr.CreatedAt;
            ReviewerId = srr.ReviewerId;
            SenderId = srr.SenderId;
            ReviewerComment = srr.ReviewerComment;
            Reason = srr.Reason;
        }
    }
}