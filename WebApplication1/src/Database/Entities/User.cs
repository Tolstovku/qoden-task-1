using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qoden.Validation;

namespace WebApplication1.Database.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required] 
        public string FirstName { get; set; }
        [Required] 
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        [Required] 
        public string NickName { get; set; }
        [Required] 
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public DateTime InvitedAt { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        [Required] 
        public int UserRoleId { get; set; }
        
        [ForeignKey("DepartmentId")] 
        public Department Department { get; set; }
        public SalaryRate SalaryRate { get; set; }
        public ICollection<SalaryRateRequest> SalaryRateRequests { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        [InverseProperty("User")]
        public ICollection<UserManager> UsersManagers { get; set; }
        [InverseProperty("Manager")]
        public ICollection<UserManager> ManagersUsers { get; set; }
        
        public void Validate(IValidator validator)
        {
            validator.CheckValue(Password, "Password").NotNull();
            validator.CheckValue(FirstName, "FirstName").NotNull();
            validator.CheckValue(Lastname, "Lastname").NotNull();
            validator.CheckValue(Patronymic, "Patronymic").NotNull();
            validator.CheckValue(NickName, "NickName").NotNull();
            validator.CheckValue(Email, "Email").NotNull();
            validator.CheckValue(DepartmentId, "Department Id").NotNull();
            validator.CheckValue(UserRoleId, "UserRole Id").NotNull();
            validator.Throw();
        }
    }
}