using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Boxing.Contracts.Extensions
{
    public static class AuthorizationExtensions
    {
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] byteArr = new byte[32];
            rng.GetBytes(byteArr);

            return Convert.ToBase64String(byteArr);
        }

        public static string CreatePasswordHash(string password, string salt)
        {
            string passwordSalt = String.Concat(password, salt);
            return string.Join("", new MD5CryptoServiceProvider().ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(passwordSalt))).Select(x => x.ToString("X2")));
        }
    }
}
