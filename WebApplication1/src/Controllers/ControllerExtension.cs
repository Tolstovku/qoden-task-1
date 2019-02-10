using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Database.Entities.Controllers
{
    public static class ControllerExtension
    {
        public static int GetLoggedUserId(this Controller controller)
        {
            return int.Parse(
                controller.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => c.Value).SingleOrDefault()
            );
        }
        
    }
}