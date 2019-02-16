using Qoden.Validation;
using WebApplication1.Validation;

namespace WebApplication1.Requests
{
    public class LoginRequest : IValidate
    {
        public string NicknameOrEmail { get; set; }
        public string Password { get; set; }
        
        public void Validate(IValidator validator)
        {
            validator.CheckValue(NicknameOrEmail, "Nickname or Email").NotNull();
            validator.CheckValue(Password, "Password").NotNull();
            validator.Throw();
        }
    }
    
    
}