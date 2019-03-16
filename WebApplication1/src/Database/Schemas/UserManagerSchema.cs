namespace WebApplication1.Database.Schemas
{
    public static class UserManagerSchema
    {
        public static string Table { get; } = "user_managers";
        public static string UserId { get; } = "user_id";
        public static string ManagerId { get; } = "manager_id";
    }
}