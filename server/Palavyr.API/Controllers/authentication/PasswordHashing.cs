using System;
using System.Security.Cryptography;

namespace Palavyr.API.Controllers
{
    /// <summary>
    /// https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    /// </summary>
    public static class PasswordHashing
    {
        private const int Iterations = 20000; 
        public static string CreateHashedPassword(string password)
        {
            
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(20);
            
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public static bool ComparePasswords(string savedPasswordHash, string incomingPasswordRaw)
        {
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(incomingPasswordRaw, salt, Iterations);
            var hash = pbkdf2.GetBytes(20);
            for (var i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}