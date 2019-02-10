using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Database.Entities
{
    public static class PasswordGenerator
    {
        public static string HashPassword(string password)
        {
            return new PasswordHasher<User>().HashPassword(null, password);
            
        }

        public static PasswordVerificationResult VerifyPassword(string providedPassword, string hashedPassword)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(null, hashedPassword, providedPassword);
        }
    }
}