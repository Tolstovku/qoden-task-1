using System;

namespace WebApplication1.Database.Entities.Requests
{
    public class GetSalaryRateRequestsByUserResponse
    {
        public int SuggestedRate { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ReviewerId { get; set; }
        public int SenderId { get; set; }
        public string ReviewerComment { get; set; }
        public string Reason { get; set; }

        public GetSalaryRateRequestsByUserResponse(SalaryRateRequest srr)
        {
            SuggestedRate = srr.SuggestedRate;
            Status = srr.Status;
            CreatedAt = srr.CreatedAt;
            ReviewerId = srr.ReviewerId;
            SenderId = srr.SenderId;
            ReviewerComment = srr.ReviewerComment;
            Reason = srr.Reason;
        }
    }
}