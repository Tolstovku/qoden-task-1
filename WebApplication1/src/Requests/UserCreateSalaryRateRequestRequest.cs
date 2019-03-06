using Qoden.Validation;
using WebApplication1.Database.Entities;

namespace WebApplication1.Requests
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
            validator.CheckValue(SuggestedRate, "Suggested rate").NotNull().Greater(0);
            validator.CheckValue(Reason, "Reason").NotNull();
            validator.Throw();
        }
    }
}