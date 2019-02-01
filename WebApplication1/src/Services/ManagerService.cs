using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IManagerService
    {
        void ModifyUser(User user);
        GetUserInfoResponse GetUserInfo(int userId);
    }

    public class ManagerService : IManagerService
    {
        private readonly DatabaseContext _db;

        public ManagerService(DatabaseContext db)
        {
            _db = db;
        }

        public void ModifyUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        public GetUserInfoResponse GetUserInfo(int userId)
        {
            return new GetUserInfoResponse(_db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId));
            _db.Users.Find(userId);
        }

    }
}
