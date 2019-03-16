using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Schemas;

namespace WebApplication1.Database.Repositories
{
    public static class SalaryRateRequestRepository
    {
        public static async Task<List<SalaryRateRequest>> GetSalaryRequestsByManagerId(this IDbConnectionFactory db, int managerId )
        {
            List<SalaryRateRequest> salaryRequests;
            var sql = $@"
                SELECT * FROM {SalaryRequestSchema.Table}
                WHERE {SalaryRequestSchema.SenderId} IN (
                    SELECT {UserManagerSchema.UserId} FROM {UserManagerSchema.Table}
                    WHERE {UserManagerSchema.ManagerId} = @managerId;);";

            using (var conn = db.GetOpenedConnection())
            {
                salaryRequests = (await conn.QueryAsync<SalaryRateRequest>(sql, new {managerId})).ToList();
            }

            return salaryRequests;
        }

        public static async Task<List<SalaryRateRequest>> GetSalaryRequestsByChainIdNewestFirst(this IDbConnectionFactory db, int chainId )
        {
            List<SalaryRateRequest> salaryRequests;
            var sql = $@"
                SELECT * FROM {SalaryRequestSchema.Table}
                WHERE {SalaryRequestSchema.RequestChainId} = @chainId
                ORDER BY {SalaryRequestSchema.CreatedAt} ASC;";
            using (var conn = db.GetOpenedConnection())
            {
                salaryRequests = (await conn.QueryAsync<SalaryRateRequest>(sql, new {chainId})).ToList();
            }

            return salaryRequests;
        }

        public static async Task<List<SalaryRateRequest>> GetAllSalaryRequests(this IDbConnectionFactory db)
        {
            List<SalaryRateRequest> salaryRequests;
            var sql = $@"
                SELECT * FROM {SalaryRequestSchema.Table};";
            using (var conn = db.GetOpenedConnection())
            {
                salaryRequests = (await conn.QueryAsync<SalaryRateRequest>(sql)).ToList();
            }

            return salaryRequests;
        }

        public static async Task<List<SalaryRateRequest>> GetSalaryRequestsByUserId(this IDbConnectionFactory db, int userId)
        {
            List<SalaryRateRequest> salaryRequests;
            var sql = $@"
                SELECT * FROM {SalaryRequestSchema.Table}
                WHERE {SalaryRequestSchema.SenderId} = @userId;";
            using (var conn = db.GetOpenedConnection())
            {
                salaryRequests = (await conn.QueryAsync<SalaryRateRequest>(sql, new {userId})).ToList();
            }

            return salaryRequests;
        }

        public static async Task InsertSalaryRequest(this IDbConnectionFactory db, SalaryRateRequest req)
        {
            var sql = $@"INSERT INTO {SalaryRequestSchema.Table}
                ({SalaryRequestSchema.RequestChainId}, {SalaryRequestSchema.SuggestedRate},
                {SalaryRequestSchema.SalaryRateRequestStatus},
                {SalaryRequestSchema.CreatedAt}, {SalaryRequestSchema.ReviewerId},
                {SalaryRequestSchema.SenderId},{SalaryRequestSchema.ReviewerComment},
                {SalaryRequestSchema.InternalComment}, {SalaryRequestSchema.Reason})
                values (@{nameof(req.RequestChainId)}, @{nameof(req.SuggestedRate)},
                @{nameof(req.SalaryRateRequestStatus)}, @{nameof(req.CreatedAt)},
                @{nameof(req.ReviewerId)}, @{nameof(req.SenderId)}, @{nameof(req.ReviewerComment)},
                @{nameof(req.InternalComment)}, @{nameof(req.Reason)});";
            using (var conn = db.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql, req);
            }
        }
    }
}