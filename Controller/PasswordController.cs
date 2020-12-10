using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Model.Enums;

namespace Controller
{
    public class PasswordController
    {
        private static string salt = ConfigurationManager.AppSettings["MD5SALT"];

        /**
         * Transform A password to an md5, hashed password
         *
         * @param password Password string to encrypt
         *
         * @return string : Encrypted password, md5 salted hash
         */
        public static string EncryptPassword(string password)
        {
            var provider = (HashAlgorithm)CryptoConfig.CreateFromName("MD5");
            var bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password));
            var computedHash = BitConverter.ToString(bytes);

            return computedHash.Replace("-", "");
        }

        /**
         * Get the password strength of a string
         * Check multiple test cases, and increment score if check is successful
         *
         * @param password Password to check strength
         *
         * @ return PasswordScore : Strength of password
         */
        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+", RegexOptions.ECMAScript))
                score++;
            if (Regex.IsMatch(password, @"[a-zA-Z]+", RegexOptions.ECMAScript))
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]+", RegexOptions.ECMAScript))
                score++;

            return (PasswordScore)score;
        }
    }
}
