using Qoden.Validation;
using WebApplication1.Database.Entities;

namespace WebApplication1.Requests
{
    public class CreateUserRequest
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int UserRoleId { get; set; }

        public User CreateUserFromRequest()
        {
            Validate(new Validator());
            return new User
            {
                Password = PasswordGenerator.HashPassword(Password),
                FirstName = FirstName,
                Lastname = Lastname,
                Patronymic = Patronymic,
                NickName = NickName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Description = Description,
                DepartmentId = DepartmentId,
                UserRoleId = UserRoleId,
                InvitedAt = TimeProvider.GetDateTime()
            };
        }
        
        public void Validate(IValidator validator)
        {
            validator.CheckValue(Password, "Password").MinLength(9).MaxLength(20).IsPassword();
            validator.CheckValue(FirstName, "FirstName").NotNull();
            validator.CheckValue(Lastname, "Lastname").NotNull();
            validator.CheckValue(NickName, "NickName").NotNull().MaxLength(15);
            validator.CheckValue(Email, "Email").NotNull().IsEmail();
            validator.CheckValue(DepartmentId, "Department Id").NotNull();
            validator.CheckValue(UserRoleId, "UserRole Id").NotNull();
            validator.Throw();
        }
    }
}