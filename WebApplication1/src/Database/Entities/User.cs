using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qoden.Validation;
using WebApplication1.Requests;

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
        public string PhoneNumber { get; set; }
        public DateTime InvitedAt { get; set; }
        public string Description { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public SalaryRate SalaryRate { get; set; }
        public ICollection<SalaryRateRequest> SalaryRateRequests { get; set; }
        [InverseProperty("User")]
        public ICollection<UserRole> UserRoles { get; set; }
        [InverseProperty("User")]
        public ICollection<UserManager> UsersManagers { get; set; }
        [InverseProperty("Manager")]
        public ICollection<UserManager> ManagersUsers { get; set; }


        public void ModifyUser(ModifyUserRequest req)
        {
            FirstName = req.FirstName ?? FirstName;
            Lastname = req.Lastname ?? Lastname;
            Patronymic = req.Patronymic ?? Patronymic;
            NickName = req.NickName ?? NickName;
            Email = req.Email ?? Email;
            PhoneNumber = req.PhoneNumber ?? PhoneNumber;
            Description = req.Description ?? Description;
            DepartmentId = req.DepartmentId ?? DepartmentId;
        }
    }
}