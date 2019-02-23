using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Qoden.Validation.AspNetCore;
using WebApplication1.Configuration;
using WebApplication1.Database.Entities;
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
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/v1/login";
                    options.LogoutPath = "/api/v1/logout";
                    options.Events.OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                });
            ConfigureDatabase(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}