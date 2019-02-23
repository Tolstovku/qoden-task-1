using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Database;

namespace WebApplication1
{
    public partial class Startup
    {
        public void ConfigureDatabase(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql();
            Debug.WriteLine(Configuration["ConnectionString"]);
            services.AddDbContext<DatabaseContext>((provider, options) =>
            {
                options.UseNpgsql(Configuration["Database:ConnectionString"]);
            }, ServiceLifetime.Singleton);
        }
    }
}