using System;
using Qoden.Validation;
using WebApplication1.Database.Entities;

namespace WebApplication1.Requests
{
    public class ModifyUserRequest
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int? DepartmentId { get; set; }

        public void Validate(IValidator validator)
        {
            if (Email != null)
                validator.CheckValue(Email, "Email").IsEmail();
            if (NickName != null)
                validator.CheckValue(NickName, "NickName").MaxLength(15);
            validator.Throw();
        }
        
        public ModifyUserRequest()
        {
        }
    }
}