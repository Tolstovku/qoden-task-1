namespace WebApplication1.Database.Schemas
{
    public static class SalaryRequestSchema
    {

        public static string Table { get; } = "salary_rate_requests";
        public static string Id { get; } = "id";
        public static string RequestChainId { get; } = "request_chain_id";
        public static string SuggestedRate { get; } = "suggested_rate";
        public static string SalaryRateRequestStatus { get; } = "status";
        public static string CreatedAt { get; } = "created_at";
        public static string ReviewerId { get; } = "reviewer_id";
        public static string SenderId { get; } = "sender_id";
        public static string ReviewerComment { get; } = "reviewer_comment";
        public static string InternalComment { get; } = "internal_comment";
        public static string Reason { get; } = "reason";
    }
}