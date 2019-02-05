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
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100);
        var hash = pbkdf2.GetBytes(20);
        var hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        return Convert.ToBase64String(hashBytes);
        }
            
        
    }
}