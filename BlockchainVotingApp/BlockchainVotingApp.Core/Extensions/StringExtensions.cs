using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainVotingApp.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Sha256(this string input)
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

        /// <summary>
        /// Compress the <paramref name="input"/> using gzip.
        /// </summary>
        public static string Compress(this string input)
        {
            // Initialise the streams used for compression.
            using var memory = new MemoryStream();
            using var stream = new GZipStream(memory, CompressionMode.Compress);

            // Write the raw input to gzip stream so it will be compressed.
            stream.Write(Encoding.UTF8.GetBytes(input));
            stream.Flush();

            memory.Seek(0, SeekOrigin.Begin);

            // Retrieve compressed data from stream and encode it base 64.
            var compressed = Convert.ToBase64String(memory.ToArray());

            return compressed;
        }

        /// <summary>
        /// Decompress the <paramref name="input"/> using gzip and return the response encoded as base64 string.
        /// </summary>
        public static string Decompress(this string input)
        { 
            // Initialise the stream which contains compressed input.
            using var compressedData = new MemoryStream();

            compressedData.Write(Convert.FromBase64String(input));

            compressedData.Flush();
            compressedData.Seek(0, SeekOrigin.Begin);

            // Create a gzip stream to decompress the input stream.
            using var stream = new GZipStream(compressedData, CompressionMode.Decompress);

            var decompressedData = new MemoryStream();

            stream.CopyTo(decompressedData);
            stream.Flush();

            // Read from decompressed stream data and encode it base64.
            return Convert.ToBase64String(decompressedData.ToArray());
        }

    }
}
