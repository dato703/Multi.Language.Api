using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace App.Core.Utils
{
    public static class DataGenerator
    {
        private static readonly string DefaultSalt = "FerhjfGGjkf44--dsdjნბsdfsdfკTE6Gწhდფhjfasl5gHJddfjფდA23)fklssdNH";

        private static readonly char[] PasswordChars = new char[]
        { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
          'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
          '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        /// <summary>
        /// Generate Sha256 hash (64 bit) from input and salt.
        /// </summary>
        /// <param name="input"> input value. </param>
        /// <returns>
        /// The SHA 256.
        /// </returns>
        public static string GenerateSha256(string input)
        {
            var hash = new StringBuilder();

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + DefaultSalt));
            foreach (var b in bytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Generate Sha256 hash (64 bit) from input and salt
        /// </summary>
        /// <param name="input">input value</param>
        /// <param name="salt">input salt value</param>
        /// <returns></returns>
        public static string GenerateSha256(string input, string salt)
        {
            var hash = new StringBuilder();

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + salt));
            foreach (var b in bytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Generates random code
        /// </summary>
        /// <param name="minValue">Code min value</param>
        /// <param name="maxValue">Code max value</param>
        /// <returns></returns>
        public static string GenerateRandomCode(int minValue = 1000, int maxValue = 9999)
        {
            var rnd = new Random();
            return rnd.Next(minValue, maxValue).ToString();
        }

        public static string GenerateSessionId(string ipAddress, string browser)
        {
            var random = new byte[40];
            var rng = RandomNumberGenerator.Create();

            rng.GetBytes(random);

            var hash = GenerateSha256(ipAddress + browser);

            var randomString = Convert.ToBase64String(random);
            var sessionId = hash + randomString;

            return sessionId;
        }

        public static bool IsValidSessionId(string sourceSessionId, string ipAddress, string browser)
        {
            var hash = GenerateSha256(ipAddress + browser);
            var sourceHash = sourceSessionId.Substring(0, 32);
            return hash == sourceHash;
        }

        public static string GeneratePassword(int length = 8)
        {
            var rnd = new Random();
            var password = new StringBuilder(length);
            var passwordCharsSize = PasswordChars.Length - 1;

            for (var i = 0; i < length; i++)
            {
                password.Append(PasswordChars[rnd.Next(0, passwordCharsSize)]);
            }

            return password.ToString();
        }

        public static string GenerateIv()
        {
            const int length = 16;
            var buffer = new StringBuilder(16);
            var rnd = new Random();

            for (var i = 0; i < length; i++)
            {
                var c = (char)rnd.Next(97, 127);
                buffer.Append(c);
            }

            return buffer.ToString();
        }

        public static string GenerateStringRandom(int length = 10)
        {
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

    }
}
