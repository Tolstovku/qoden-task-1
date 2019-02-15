using Qoden.Validation;

namespace WebApplication1.Database.Entities.Requests
{
    public class AssignManagerRequest : IValidate
    {
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        
        
        public void Validate(IValidator validator)
        {
            validator.CheckValue(UserId, "User Id").NotNull();
            validator.CheckValue(ManagerId, "Manager Id").NotNull();
            validator.Throw();
        }
    }
}