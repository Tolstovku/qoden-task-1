using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace WebApplication1.Database.Entities
{
    public static class PasswordGenerator
    {
        public static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
        var hashBytes = pbkdf2.GetBytes(36);
        return Convert.ToBase64String(hashBytes);
        }
    }
}