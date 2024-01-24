using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IQRGenerator
    {
        public Bitmap GetCode(string data);

        public byte[] GetCodeRaw(string data);
    }
}
