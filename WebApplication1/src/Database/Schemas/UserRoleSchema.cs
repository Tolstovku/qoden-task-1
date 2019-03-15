namespace WebApplication1.Database.Schemas
{
    public static class UserRoleSchema
    {

        public static string Table { get; } = "user_roles";
        public static string UserId { get; } = "user_id";
        public static string RoleId { get; } = "role_id";
    }
}