using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Repositories
{
    public static class RoleRepository
    {
        public static async Task<List<Role>> GetRolesByUserId(this IDbConnectionFactory db, int userId)
        {
            List<Role> roles;
            var sql = $@"
                SELECT * FROM {RoleSchema.Table} as r
                INNER JOIN {UserRoleSchema.Table}  as ur on r.{RoleSchema.Id} = ur.{UserRoleSchema.RoleId}
                WHERE {UserRoleSchema.UserId} = @userId;";
            using (var conn = db.GetOpenedConnection())
            {
                roles = (await conn.QueryAsync<Role>(sql, new {userId})).ToList();
            }

            return roles;
        }

    }
}