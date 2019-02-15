using System.Threading.Tasks;
using Qoden.Validation;

namespace WebApplication1.Database.Entities.Requests
{
    public class AnswerSalaryRateRequestRequest : IValidate
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

        public void Validate(IValidator validator)
        {
            validator.CheckValue(RequestChainId, "Request chain Id").NotNull();
            validator.CheckValue(SalaryRateRequestStatus, "Salary rate request status").NotDefault("{Key} must be a valid status code");
            validator.CheckValue(ReviewerId, "Reviewer Id").NotNull();
            validator.CheckValue(ReviewerComment, "Reviewer comment").NotNull();
            validator.Throw();
        }
    }
}
