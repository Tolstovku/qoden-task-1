using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1
{
    public static class UserInfoProvider
    {

        public static string GetUserId(HubCallerContext ctx)
        {
            return ctx.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        }

        public static string GetUsername(HubCallerContext ctx)
        {
            return ctx.User.Claims.Single(claim => claim.Type.Equals(ClaimTypes.Name)).Value;
        }
    }
}