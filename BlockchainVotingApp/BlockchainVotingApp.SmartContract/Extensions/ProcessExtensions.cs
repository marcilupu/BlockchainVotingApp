using System.Diagnostics;

namespace BlockchainVotingApp.SmartContract.Extensions
{
    /// <summary>
    /// Contains extensions for process class.
    /// </summary>
    internal static class ProcessExtensions
    {
        /// <summary>
        /// Invoke a .bat script and return the return code as result.
        /// </summary>
        /// <param name="batFile">Bat file which will be executed</param>
        /// <param name="arguments">Arguments passed to the bat file.</param>
        /// <returns>Return code of the bat execution or null if any error occures.</returns>
        public static async Task<int?> InvokeBat(this Process process, string batFile, params string[] arguments)
        {
            try
            {
                process.StartInfo.FileName = batFile;
                
                foreach (var argument in arguments)
                {
                    process.StartInfo.ArgumentList.Add(argument);
                }

                process.Start();

                await process.WaitForExitAsync();

                if (process.HasExited && process.ExitCode != 0)
                {
                    return null;
                }

                return process.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process cannot be invoked. The error is: {ex}");

                return null;
            }
        }
    }
}
