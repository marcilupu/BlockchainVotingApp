using System.Security.Cryptography;
using System.Text;

namespace BlockchainVotingApp.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ComputeSha256Hash(this string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("0x");
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
