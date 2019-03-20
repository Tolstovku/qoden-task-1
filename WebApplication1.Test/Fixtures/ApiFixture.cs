using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Extensions;
using WebApplication1;
using WebApplication1.Database;
using Xunit;

namespace Tests.Fixtures
{
    public class ApiFixture : IAsyncLifetime, IDisposable
    {
        public TestServer Server { get; set; }
        public HttpClient RegularUser { get; set; }
        public HttpClient ManagerUser { get; set; }
        public HttpClient AdminUser { get; set; }
        public DatabaseContext Db => Server.Host.Services.GetService<DatabaseContext>();
        public IDbConnectionFactory ConnectionFactory => Server.Host.Services.GetService<IDbConnectionFactory>();
        public HubConnection Hub { get; set; }
        public string ProjectDir = Path.Combine(Directory.GetCurrentDirectory(), @"../../../");

        public ApiFixture()
        {
            var builder = SetupWebHost();
            Server = new TestServer(builder);
            RegularUser = Server.CreateClient();
            ManagerUser = Server.CreateClient();
            AdminUser = Server.CreateClient();
            var something = Server.BaseAddress;

            if (Debugger.IsAttached)
            {
                RegularUser.Timeout = TimeSpan.FromHours(1);
                ManagerUser.Timeout = TimeSpan.FromHours(1);
                AdminUser.Timeout = TimeSpan.FromHours(1);
            }
        }

        public async Task InitializeAsync()
        {
            await SetupDb();
            var cookies = await RegularUser.AuthorizeClient("User", "123");
            await ManagerUser.AuthorizeClient("Manager", "123");
            await AdminUser.AuthorizeClient("Admin", "123");

            Hub = new HubConnectionBuilder().WithUrl(new Uri(Server.BaseAddress, "ws/chat"), options =>
                {
                    options.HttpMessageHandlerFactory = _ => Server.CreateHandler();
                    options.Headers.Add("Cookie", cookies);
                })
                .Build();
        }

        public async Task DisposeAsync()
        {
            await ClearDb();
        }

        public void Dispose()
        {
            RegularUser.Dispose();
            ManagerUser.Dispose();
            AdminUser.Dispose();
            Server.Dispose();
        }


        public async Task SetupDb()
        {
            string sql = File.ReadAllText(Path.Combine(ProjectDir, "setup.sql"));
            using (var conn = ConnectionFactory.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql);
            }
        }

        public async Task ClearDb()
        {
            string sql = "drop schema public cascade; create schema public;";
            using (var conn = ConnectionFactory.GetOpenedConnection())
            {
                await conn.ExecuteAsync(sql);
            }
        }

        private IWebHostBuilder SetupWebHost()
        {
            return WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    builder
                        .SetBasePath(ProjectDir)
                        .AddJsonFile("config.json");
                })
                .UseStartup<Startup>();
        }
    }
}