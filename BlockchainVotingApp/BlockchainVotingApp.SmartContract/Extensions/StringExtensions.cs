using System.Security.Cryptography;
using System.Text;

namespace BlockchainVotingApp.SmartContract.Extensions
{
    /// <summary>
    /// Contains extensions methods for string variables.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Copy all files from source directory path to target directory path (recursively).
        /// </summary>
        /// <param name="sourceDirectoryPath">Path of the directory used as source.</param>
        /// <param name="targetDirectoryPath">Path of the directory used as target.</param>
        public static bool TryCopyTo(this string sourceDirectoryPath, string targetDirectoryPath)
        {
            try
            {
                foreach (string dirPath in Directory.GetDirectories(sourceDirectoryPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourceDirectoryPath, targetDirectoryPath));
                }

                foreach (string newPath in Directory.GetFiles(sourceDirectoryPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(sourceDirectoryPath, targetDirectoryPath), true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

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
