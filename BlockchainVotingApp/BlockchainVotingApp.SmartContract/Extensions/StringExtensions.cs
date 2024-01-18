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
    }
}
