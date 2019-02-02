using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Database.Entities
{
    public class UserManager
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public int ManagerId { get; set; }
  
        public User User { get; set; }
        public User Manager { get; set; }

        public UserManager(int userId, int managerId)
        {
            UserId = userId;
            ManagerId = managerId;
        }
            
    }
}