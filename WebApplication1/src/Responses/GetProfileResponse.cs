using System;

namespace WebApplication1.Database.Entities.Requests
{
    public class GetProfileResponse
    {
        public GetProfileResponse(User user)
        {
            FirstName = user.FirstName;
            Lastname = user.Lastname;
            Patronymic = user.Patronymic;
            NickName = user.NickName;
            Email = user.Email;
            Description = user.Description;
            DepartmentName = user.Department.Name;
        }

        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
    }
}