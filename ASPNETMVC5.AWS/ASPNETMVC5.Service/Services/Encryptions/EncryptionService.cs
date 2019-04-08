using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Service.Services.Encryptions
{
    public class EncryptionService
    {
        /// <summary>
        /// Creates a random password salt
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomSalt()
        {
            var saltBytes = new Byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hash a password using a random salt.
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashPassword(string pass, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Encoding.Unicode.GetBytes(salt);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            var algorithm = HashAlgorithm.Create("SHA1");

            if (algorithm == null)
                return String.Empty;

            var inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        /// <summary>
        /// generates a random password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int length)
        {
            var r = new Random(Environment.TickCount);
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
                builder.Append(chars[r.Next(chars.Length)]);

            return builder.ToString();
        }
    }
}
