using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Database.Entities
{
    public class SalaryRate
    {
        public int Id { get; set; }

        [Required] 
        public int Rate { get; set; }

        [Required] 
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserId")] 
        public User User { get; set; }
    }
}