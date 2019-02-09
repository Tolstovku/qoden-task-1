using System;

namespace WebApplication1.Database.Entities
{
    public class TimeProvider
    {
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}