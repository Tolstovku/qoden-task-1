using Qoden.Validation;

namespace WebApplication1.Database.Entities.Requests
{
    public class UserCreateSalaryRateRequestRequest: IValidate
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
        
        public void Validate(IValidator validator)
        {
            validator.CheckValue(SenderId, "User Id").NotNull();
            validator.CheckValue(SuggestedRate, "Suggested rate").NotNull();
            validator.CheckValue(Reason, "Reason").NotNull();
            validator.Throw();
        }
    }
}