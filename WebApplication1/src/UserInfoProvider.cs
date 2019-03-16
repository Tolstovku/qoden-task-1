using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1
{
    public static class UserInfoProvider
    {
        public static string GetUsername(HubCallerContext ctx)
        {
            return ctx.User.Identity.Name;
        }
    }
}