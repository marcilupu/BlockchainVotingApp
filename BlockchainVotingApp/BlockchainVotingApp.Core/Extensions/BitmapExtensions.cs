using System.Drawing;


namespace BlockchainVotingApp.Core.Extensions
{
    public static class BitmapExtensions
    {
        private static byte[] BitmapToBytes(this Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
#pragma warning disable CA1416 // Validate platform compatibility
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
#pragma warning restore CA1416 // Validate platform compatibility
                return stream.ToArray();
            }
        }
    }
}
