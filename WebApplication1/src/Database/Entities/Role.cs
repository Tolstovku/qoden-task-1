using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Entities
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column()]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles;
    }
}