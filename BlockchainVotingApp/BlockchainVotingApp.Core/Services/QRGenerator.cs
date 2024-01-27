using BlockchainVotingApp.Core.Extensions;
using BlockchainVotingApp.Core.Infrastructure;
using QRCoder;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace BlockchainVotingApp.Core.Services
{
    internal class QRGenerator : IQRGenerator
    {
        public Bitmap GetCode(string data)
        {
            using var raw = new MemoryStream(CreateCodeInternal(data));

#pragma warning disable CA1416 // Validate platform compatibility
            return new Bitmap(raw);
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public byte[] CreateCode(string data) => CreateCodeInternal(data);

        public string GetCodeContent(Stream data)
        {
#pragma warning disable CA1416 // Validate platform compatibility

            var bitmap = new Bitmap(data);
            var luminanceSource = new BitmapLuminanceSource(bitmap);
            var source = new BinaryBitmap(new HybridBinarizer(luminanceSource));

#pragma warning restore CA1416 // Validate platform compatibility


            var reader = new QRCodeReader();
            var result = reader.decode(source);

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                return result.Text.Decompress();
            }

            throw new ApplicationException("Failed to decode provided image");
        }

        #region Private

        private byte[] CreateCodeInternal(string data)
        {
            // Compress the input so it will use less bytes in the final result.
            var compressed = data.Compress();

            // Initialise the objects required to create the qr code.
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(compressed, QRCodeGenerator.ECCLevel.H);
            var qrCode = new BitmapByteQRCode(qrCodeData);

            // Get graphic bytes and return.
            var imageBytes = qrCode.GetGraphic(12);

            return imageBytes;
        }

        #endregion
    }
}
