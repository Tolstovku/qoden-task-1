using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Repositories
{
    public static class DepartmentRepository
    {

        public static async Task<Department> GetDepartmentById(this IDbConnectionFactory db, int depId)
        {
            Department user;
            var sql = $@"
                SELECT * FROM {DepartmentSchema.Table}
                WHERE {DepartmentSchema.Id} = @depId;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<Department>(sql, new {depId})).SingleOrDefault();
            }

            return user;
        }
    }
}