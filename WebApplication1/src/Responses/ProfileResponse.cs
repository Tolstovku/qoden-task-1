using WebApplication1.Database.Entities;

namespace WebApplication1.Responses
{
    public class ProfileResponse
    {
        public ProfileResponse(User user)
        {
            FirstName = user.FirstName;
            Lastname = user.Lastname;
            Patronymic = user.Patronymic;
            NickName = user.NickName;
            Email = user.Email;
            Description = user.Description;
            DepartmentName = user.Department.Name;
        }
        
        public ProfileResponse(){}

        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
    }
}