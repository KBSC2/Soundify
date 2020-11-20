using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Controller
{
    public class PasswordController
    {
        private static string salt = "Sterk_W@chtw00rd2";

        public static string EncryptPassword(string password)
        {
            var provider = (HashAlgorithm)CryptoConfig.CreateFromName("MD5");
            var bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password));
            var computedHash = BitConverter.ToString(bytes);

            return computedHash.Replace("-", "");
        }

        public static bool PasswordsAreEqual(string password, string hash)
        {
            var passwordHash = EncryptPassword(password);
            return passwordHash.Equals(hash);
        }

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

        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }
    }
}
