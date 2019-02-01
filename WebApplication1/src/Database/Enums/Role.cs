using System;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Database.Entities
{
    [Flags]
    public enum Role
    {
        User = 0,
        Manager = 1,
        Admin = 2
    }

    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public MyAuthorizeAttribute(Role roles)
        {
            Roles = roles.ToString();
        }
    }
}