using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Repositories
{
    public static class UserRepository
    {
        public static async Task<User> GetUserById(this IDbConnectionFactory db, int userId)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table}
                WHERE {UserSchema.Id} = @userId;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User>(sql, new {userId})).SingleOrDefault();
            }

            return user;
        }

        public static async Task<User> GetUserByIdIncludeDepartment(this IDbConnectionFactory db, int userId)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table} as u
                INNER JOIN {DepartmentSchema.Table}  as d on u.{UserSchema.Id} = d.{DepartmentSchema.Id}
                WHERE u.{UserSchema.Id} = @userId;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User, Department, User>(sql,
                    (u, d) =>
                    {
                        u.Department = d;
                        return u;
                    },
                    new {userId})).SingleOrDefault();
            }

            return user;
        }

        public static async Task<User> GetUserByEmailOrNickname(this IDbConnectionFactory db, string nickOrEmail)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table}
                WHERE {UserSchema.Email} = @nickOrEmail OR {UserSchema.NickName} = @nickOrEmail;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User>(sql, new {nickOrEmail})).SingleOrDefault();
            }

            return user;
        }

        public static async Task<User> GetUserByEmail(this IDbConnectionFactory db, string email)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table}
                WHERE {UserSchema.Email} = @email;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User>(sql, new {email})).SingleOrDefault();
            }

            return user;
        }

        public static async Task<User> GetUserByPhoneNumber(this IDbConnectionFactory db, string phoneNumber)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table}
                WHERE {UserSchema.PhoneNumber} = @phoneNumber;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User>(sql, new {phoneNumber})).SingleOrDefault();
            }

            return user;
        }

        public static async Task<User> GetUserByNickname(this IDbConnectionFactory db, string nickname)
        {
            User user;
            var sql = $@"
                SELECT * FROM {UserSchema.Table}
                WHERE {UserSchema.NickName} = @nickname;";
            using (var conn = db.GetOpenedConnection())
            {
                user = (await conn.QueryAsync<User>(sql, new {nickname})).SingleOrDefault();
            }

            return user;
        }

        public static async Task InsertUser(this IDbConnectionFactory db, User user)
        {
            var sql = $@"INSERT INTO {UserSchema.Table}
                ({UserSchema.Password}, {UserSchema.FirstName}, {UserSchema.Lastname}, {UserSchema.Patronymic},
                {UserSchema.NickName}, {UserSchema.Email}, {UserSchema.PhoneNumber},
                {UserSchema.InvitedAt}, {UserSchema.Description}, {UserSchema.DepartmentId})
                values (@{nameof(user.Password)}, @{nameof(user.FirstName)}, @{nameof(user.Lastname)}, @{nameof(user.Patronymic)},
                @{nameof(user.NickName)}, @{nameof(user.Email)}, @{nameof(user.PhoneNumber)},
                @{nameof(user.InvitedAt)}, @{nameof(user.Description)}, @{nameof(user.DepartmentId)});";

            using (var conn = db.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql, user);
            }
        }

        public static async Task UpdateUser(this IDbConnectionFactory db, User user)
        {
            var sql = $@"UPDATE {UserSchema.Table} SET
                {UserSchema.Password} = @{nameof(user.Password)},
                {UserSchema.FirstName} = @{nameof(user.FirstName)},
                {UserSchema.Lastname} = @{nameof(user.Lastname)},
                {UserSchema.Patronymic} = @{nameof(user.Patronymic)},
                {UserSchema.NickName} = @{nameof(user.NickName)},
                {UserSchema.Email} = @{nameof(user.Email)},
                {UserSchema.PhoneNumber} = @{nameof(user.PhoneNumber)},
                {UserSchema.InvitedAt} = @{nameof(user.InvitedAt)},
                {UserSchema.Description} = @{nameof(user.Description)},
                {UserSchema.DepartmentId} = @{nameof(user.DepartmentId)}
                WHERE {UserSchema.Id} = @{nameof(user.Id)};";
            using (var conn = db.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql, user);
            }
        }




    }
}