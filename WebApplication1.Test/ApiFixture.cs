using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1;
using WebApplication1.Database;
using WebApplication1.Requests;
using Xunit;

namespace Tests
{
    public class ApiFixture : IAsyncLifetime, IDisposable
    {
        public TestServer Server { get; set; }
        public HttpClient RegularUser { get; set; }
        public HttpClient ManagerUser { get; set; }
        public HttpClient AdminUser { get; set; }
        public DatabaseContext Db => Server.Host.Services.GetService<DatabaseContext>();

        public  ApiFixture()
        {
            var builder = SetupWebHost();
            Server = new TestServer(builder);
            RegularUser = Server.CreateClient();
            ManagerUser = Server.CreateClient();
            AdminUser = Server.CreateClient();

            if (Debugger.IsAttached)
            {
                RegularUser.Timeout = TimeSpan.FromHours(1);
                ManagerUser.Timeout = TimeSpan.FromHours(1);
                AdminUser.Timeout = TimeSpan.FromHours(1);
            }
        }

        private async Task AuthorizeUser(HttpClient client, string nicknameOrEmail, string password)
        {
            var request = new LoginRequest
            {
                NicknameOrEmail = nicknameOrEmail,
                Password = password
            };

            var response = await client.PostAsJsonAsync("api/v1/login", request);
            response.StatusCode.Should().Be(200);
            var cookie = response.Headers.FirstOrDefault(h => h.Key.Equals("Set-Cookie")).Value;
            client.DefaultRequestHeaders.Add("Cookie", cookie);
        }

        public async Task InitializeAsync()
        {
            await AuthorizeUser(RegularUser, "User", "123");
            await AuthorizeUser(ManagerUser, "Manager", "123");
            await AuthorizeUser(AdminUser, "Admin", "123");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            RegularUser.Dispose();
            ManagerUser.Dispose();
            AdminUser.Dispose();
            Server.Dispose();
        }

        private static IWebHostBuilder SetupWebHost()
        {
            var projectDir = System.IO.Directory.GetCurrentDirectory();
            var windowsBasePath = "C:/Users/Daniil/iCloudDrive/Qoden/task1/qoden-task-1/WebApplication1";
            return WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, builder) => { builder
                    .SetBasePath(projectDir)
                    .AddJsonFile("config.json"); })
                .UseStartup<Startup>();
        }
    }
}