using System;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace HGGM.Services.Discourse
{
    public class Crypto
    {
        public static string ConvertBase64ToString(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        private static string ConvertBytesToString(byte[] value)
        {
            return BitConverter.ToString(value).Replace("-", string.Empty).ToLower();
        }

        public static string ConvertStringToBase64(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        private static byte[] ConvertStringToBytes(string value)
        {
            var aSciiEncoding = new ASCIIEncoding();
            return aSciiEncoding.GetBytes(value);
        }

        public static string CreateHmacsha256(string secret, string message)
        {
            var hMacsha = new HMACSHA256(ConvertStringToBytes(secret));
            return ConvertBytesToString(hMacsha.ComputeHash(ConvertStringToBytes(message)));
        }

        public static bool IsSignatureValid([NotNull] string secret, [NotNull] string sso, [NotNull] string sig)
        {
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(sso) || string.IsNullOrEmpty(sig))
                return false;
            return sig == CreateHmacsha256(secret, sso);
        }
    }
}