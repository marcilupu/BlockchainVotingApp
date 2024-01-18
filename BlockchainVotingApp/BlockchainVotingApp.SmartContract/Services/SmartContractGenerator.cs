using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using BlockchainVotingApp.SmartContract.Utilities;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal sealed class SmartContractGenerator : ISmartContractGenerator
    {
        private readonly ISmartContractConfiguration _configuration;

        public SmartContractGenerator(ISmartContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generate the election context : election context, verifier zok file, zokrates context
        /// </summary>
        /// <param name="contextIdentifier"></param>
        /// <param name="userIds"></param>
        /// <returns>Contract metadata</returns>
        public async Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> usersIds)
        {
            // 1. Create election smart contract context
            string electionPath = Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier);
            string templatePath = Path.Combine(_configuration.GeneratorWorkspace, "template");

            DirectoryHelper.CopyFilesRecursively(templatePath, electionPath);

            // 2. Create verifier zok file
            string verifierZokPath = GenerateVerifierFile(usersIds.ToList(), electionPath);

            // 3. Setup verifier zokrates
            string cmd = Path.Combine(_configuration.GeneratorWorkspace, _configuration.ContextGenerator);
            var response = ProcessHelper.InvokeProcess(cmd, electionPath);
            if (response == 0)
                return null;

            // 4. Get contract metadata (abi, bytecode)
            var contractMetadata = await GetSmartContractMetadata(electionPath);

            return contractMetadata;
        }

        public Task GenerateProof(string contextIdentifier, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ContractMetadata?> GetSmartContractMetadata(string contextIdentifier)
        {
            string abiPath = Path.Combine(contextIdentifier, _configuration.ABI);
            string bytecodePath = Path.Combine(contextIdentifier, _configuration.Bytecode);

            var abi = await File.ReadAllTextAsync(abiPath);
            var bytecode = await File.ReadAllTextAsync(bytecodePath);

            var contractMetadata = new ContractMetadata(bytecode.Trim('"'), abi);

            return contractMetadata;
        }

        public async Task<string> DeploySmartContract(string contextIdentifier, string adminAccountPrivateKey)
        {
            string adminDefaultAccountPrivateKey = adminAccountPrivateKey;

            if (string.IsNullOrEmpty(adminDefaultAccountPrivateKey))
            {
                adminDefaultAccountPrivateKey = _configuration.AdminDefaultAccountPrivateKey;
            }

            var account = new Account(adminDefaultAccountPrivateKey);
            var web3 = new Web3(account, _configuration.BlockchainNetworkUrl);

            string fullContextIdentifier = Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier);
            var contextMetadata = await GetSmartContractMetadata(fullContextIdentifier);

            if (contextMetadata != null)
            {
                ElectionDeployment deploymentMessage = new ElectionDeployment(contextMetadata.Bytecode);

                try
                {
                    //var estimatedgas = await web3.Eth.Transactions.EstimateGas.SendRequestAsync(new Nethereum.RPC.Eth.DTOs.CallInput
                    //{
                    //    From = _configuration.AdminDefaultAccountAddress,
                    //    Data = deploymentMessage.ByteCode
                    //});

                    var deploymentHandler = web3.Eth.GetContractDeploymentHandler<ElectionDeployment>();
                    var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);

                    var contractAddress = transactionReceipt.ContractAddress;

                    return contractAddress;
                }
                catch (RpcResponseException response)
                {
                    throw response;
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }


        #region private

        private string GenerateVerifierFile(List<int> usersIds, string contextIdentifier)
        {
            string verifierZokPath = Path.Combine(contextIdentifier, "verifier", "Verifier.zok");

            try
            {
                StreamWriter sw = new StreamWriter(verifierZokPath);

                var usersCount = usersIds.Count;

                var rand = new Random();
                var randNumber = rand.Next();

                sw.WriteLine("def main(private field userId) {");
                sw.Write($"field[{usersCount}] ids = [");
                for (int i = 0; i < usersCount - 1; i++)
                {
                    sw.Write($"{usersIds[i]},");
                }
                sw.WriteLine($"{usersIds[usersCount - 1]}];");

                sw.WriteLine($"field randomSeed = {randNumber};");
                sw.WriteLine("field mut match = randomSeed;");

                sw.WriteLine(@$"for u32 i in 0..{usersCount} " + "{");
                sw.WriteLine(" match = if ids[i] == userId { match + 1 } else { match }; ");
                sw.WriteLine("}");

                sw.WriteLine("assert(match > randomSeed);");
                sw.WriteLine("return;");
                sw.WriteLine("}");

                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine($"The file {verifierZokPath} has been generated...");
            }

            return verifierZokPath;
        }

        #endregion
    }
}
