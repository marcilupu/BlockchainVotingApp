using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainVotingApp.SmartContract.Utilities
{
    public static class ProcessHelper
    {
        public static int InvokeProcess(string cmd, string arguments)
        {
            try
            {
                Process process = new Process();

                process.StartInfo.FileName = cmd;
                process.StartInfo.Arguments = arguments;

                process.Start();

                process.WaitForExit();

                if (process.HasExited && process.ExitCode != 0)
                    return 0;

                return process.Id;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"The process cannot be invoked. The error is: {ex}");
                return 0;
            }
        }
    }
}
