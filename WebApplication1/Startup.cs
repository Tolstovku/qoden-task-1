using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Qoden.Validation.AspNetCore;
using WebApplication1.Configuration;
using WebApplication1.Database.Entities;
using WebApplication1.Hubs;
using WebApplication1.Services;

namespace WebApplication1
{
    public partial class Startup
    {
        public IConfiguration Configuration;

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ISalaryRateRequestService, SalaryRateRequestService>();
            services.Configure<Config>(Configuration.GetSection("Database"));
            services.AddMvc(o =>
            {
                o.Filters.Add<ApiExceptionFilterAttribute>();
                o.OutputFormatters.Clear();
                o.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }, ArrayPool<char>.Shared));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.Events.OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });
            services.AddSignalR(options => options.ClientTimeoutInterval = TimeSpan.FromHours(1));
            ConfigureDatabase(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseMvc();

            app.UseCors(x => x.WithOrigins("http://localhost:8000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseSignalR(routes => routes.MapHub<ChatHub>("/ws/users"));
        }
    }
}