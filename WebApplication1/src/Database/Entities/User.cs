using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Database.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        public string FirstName { get; set; }
        [Required] 
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        [Required] 
        public string NickName { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required] 
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
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
    }
}