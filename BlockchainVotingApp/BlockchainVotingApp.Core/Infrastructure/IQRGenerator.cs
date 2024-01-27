using System.Drawing;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IQRGenerator
    {
        public Bitmap GetCode(string data);

        public byte[] CreateCode(string data);

        public string GetCodeContent(Stream data);
    }
}
