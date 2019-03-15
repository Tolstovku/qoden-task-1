using Microsoft.EntityFrameworkCore.Design;

namespace WebApplication1.Database.Schemas
{
    public static class UserSchema
    {
        public static string Table { get; } = "users";
        public static string Id { get; } = "id";
        public static string Password { get; } = "password";
        public static string FirstName { get; } = "first_name";
        public static string Lastname { get; } = "last_name";
        public static string Patronymic { get; } = "patronymic";
        public static string NickName { get; } = "nickname";
        public static string Email { get; } = "email";
        public static string PhoneNumber { get; } = "phone_number";
        public static string InvitedAt { get; } = "invited_at";
        public static string Description { get; } = "description";
        public static string DepartmentId { get; } = "department_id";
    }
}