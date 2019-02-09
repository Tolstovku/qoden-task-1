namespace WebApplication1.Database.Entities.Requests
{
    public class AnswerSalaryRateRequestRequest
    {
        public int RequestChainId { get; set; }
        public SalaryRateRequestStatus SalaryRateRequestStatus { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerComment { get; set; }
        public string InternalComment { get; set; }

        public SalaryRateRequest ConvertToSalaryRateRequest()
        {
            return new SalaryRateRequest
            {
                RequestChainId = RequestChainId,
                SalaryRateRequestStatus = SalaryRateRequestStatus,
                ReviewerId = ReviewerId,
                ReviewerComment = ReviewerComment,
                InternalComment = InternalComment,
                CreatedAt = TimeProvider.GetDateTime()
            };
        }
    }
}
