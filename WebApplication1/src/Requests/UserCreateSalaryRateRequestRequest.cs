namespace WebApplication1.Database.Entities.Requests
{
    public class UserCreateSalaryRateRequestRequest
    {
        public int SenderId { get; set; }
        public int SuggestedRate { get; set; }
        public string Reason { get; set; }

        public SalaryRateRequest ConvertToSalaryRateRequest()
        {
            return new SalaryRateRequest
            {
                SuggestedRate = SuggestedRate,
                SenderId = SenderId,
                Reason = Reason,
                SalaryRateRequestStatus = SalaryRateRequestStatus.Pending,
                CreatedAt = TimeProvider.GetDateTime()
            };
        }
    }
}