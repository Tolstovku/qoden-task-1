using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Repositories
{
    public static class UserManagerRepository
    {

        public static async Task<List<UserManager>> GetAllUserManagers(this IDbConnectionFactory db)
        {
            List<UserManager> userManagers;
            var sql = $@"
                SELECT * FROM {UserManagerSchema.Table};";
            using (var conn = db.GetOpenedConnection())
            {
                userManagers = (await conn.QueryAsync<UserManager>(sql)).ToList();
            }

            return userManagers;
        }
        
        public static async Task<UserManager> GetUserManagerByIds(this IDbConnectionFactory db, int userId,int managerId)
        {
            UserManager userManager;
            var sql = $@"
                SELECT * FROM {UserManagerSchema.Table}
                WHERE {UserManagerSchema.ManagerId} = @managerId AND {UserManagerSchema.UserId} = @userId;";
            using (var conn = db.GetOpenedConnection())
            {
                userManager = (await conn.QueryAsync<UserManager>(sql, new {userId, managerId})).SingleOrDefault();
            }

            return userManager;
        }

        public static async Task InsertUserManager(this IDbConnectionFactory db, UserManager userManager)
        {
            var sql = $@"INSERT INTO {UserManagerSchema.Table}
                ({UserManagerSchema.UserId}, {UserManagerSchema.ManagerId})
                values (@{nameof(userManager.UserId)}, @{nameof(userManager.ManagerId)});";
            using (var conn = db.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql, userManager);
            }
        }

        public static async Task DeleteUserManager(this IDbConnectionFactory db, UserManager userManager)
        {
            var sql = $@"DELETE FROM {UserManagerSchema.Table}
                WHERE {UserManagerSchema.UserId} = @{nameof(userManager.UserId)} AND
                {UserManagerSchema.ManagerId} =  @{nameof(userManager.ManagerId)};";
            using (var conn = db.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql, userManager);
            }
        }
    }
}