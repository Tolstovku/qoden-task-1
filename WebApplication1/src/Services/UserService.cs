using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities.Requests;
using WebApplication1.src.Database;

namespace WebApplication1.Database.Entities.Services
{
    public interface IUserService
    {
        GetProfileResponse GetProfile(int userId);
    }

    internal class UserService : IUserService
    {
        private readonly DatabaseContext _db;

        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public GetProfileResponse GetProfile(int userId)
        {
            var user = _db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == userId);
            return new GetProfileResponse(user);
        }


    }
}