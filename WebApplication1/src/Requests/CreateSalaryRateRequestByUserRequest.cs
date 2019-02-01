namespace WebApplication1.Database.Entities.Requests
{
    public class CreateSalaryRateRequestByUserRequest
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
                Status = Status.Pending,
                CreatedAt = DateTimeCreator.getDateTime()
            };
        }
    }
}