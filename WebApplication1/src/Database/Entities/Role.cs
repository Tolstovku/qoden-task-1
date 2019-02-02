using System.Collections.Generic;

namespace WebApplication1.Database.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users;
    }
}