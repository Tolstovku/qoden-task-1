using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApplication1.Database
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetOpenedConnection();
    }

    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {

        private readonly string _connectionString;

        public NpgsqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration["Database:ConnectionString"];
        }

        public IDbConnection GetOpenedConnection()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return conn;

        }
    }
}
