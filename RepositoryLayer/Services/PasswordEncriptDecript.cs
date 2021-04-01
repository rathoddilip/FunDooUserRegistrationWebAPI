using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RepositoryLayer.Services
{
    class PasswordEncriptDecript
    {
        public static string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException("password");
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string ConvertToDecrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) 
            throw new ArgumentNullException("password");
            var base64EncodeBytes = Convert.FromBase64String(password);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length);
            return result;
        }
    }
}
