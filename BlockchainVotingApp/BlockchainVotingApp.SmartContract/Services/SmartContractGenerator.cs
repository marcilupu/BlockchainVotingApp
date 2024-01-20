using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using BlockchainVotingApp.SmartContract.Utilities;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Diagnostics;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal sealed class SmartContractGenerator : ISmartContractGenerator
    {
        private readonly ISmartContractConfiguration _configuration;
        private readonly PathsLookup _pathsLookup;

        public SmartContractGenerator(ISmartContractConfiguration configuration, PathsLookup pathsLookup)
        {
            _configuration = configuration;
            _pathsLookup = pathsLookup;
        }

        /// <summary>
        /// Generate the election context : election context, verifier zok file, zokrates context
        /// </summary>
        /// <param name="contextIdentifier"></param>
        /// <param name="usersIds"></param>
        /// <returns>A new instance of <see cref="ContractMetadata"/></returns>
        public async Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> usersIds)
        {
            // 1. Create election smart contract context setup
            string templatePath = _pathsLookup.GeneratorTemplatePath();
            string contextPath = _pathsLookup.ContextPath(contextIdentifier);
            string verifierZokPath = _pathsLookup.ContextVerifierProgramPath(contextIdentifier);


            // 2. Create verifier zok file
            var verifierProgram = VerifierProgramCreator.New(usersIds).Generate();


            if (!string.IsNullOrEmpty(verifierProgram))
            {
                // Attempt to copy the template directory content to election path directory.
                if (templatePath.TryCopyTo(contextPath))
                {
                    File.WriteAllText(verifierZokPath, verifierProgram);

                    // 3. Setup and run context generator. 
                    string generatorBat = _pathsLookup.CGeneratorBatPath();

                    var response = await new Process().InvokeBat(generatorBat, contextPath);

                    if (response != null)
                    {
                        // 4. Get contract metadata (abi, bytecode)
                        var contractMetadata = await GetSCMetadataInternal(contextIdentifier);

                        return contractMetadata;
                    }
                }
            }

            return null;
        }

        public async Task<Proof?> GenerateProof(string contextIdentifier, int userId)
        {
            // Setup the required path variables. 
            string generatorBat = _pathsLookup.PGeneratorBatPath();
            string contextPath = _pathsLookup.ContextPath(contextIdentifier);

            // Generate a new unique identifier for proof.
            string proofId = Guid.NewGuid().ToString();

            // Execute the bat and extract the proof from file.
            var response = await new Process().InvokeBat(generatorBat, contextPath, proofId, userId.ToString());

            if (response != null)
            {
                var proofPath = _pathsLookup.ContextVerifierProofPath(contextIdentifier, proofId);

                if (Proof.TryRead(proofPath, out var proof))
                {
                    return proof;
                }
            }

            return null;
        }

        public async Task<ContractMetadata?> GetSmartContractMetadata(string contextIdentifier)
        {
            try
            {
                return await GetSCMetadataInternal(contextIdentifier);
            }
            catch
            {
            }

            return null;
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

            var contextMetadata = await GetSCMetadataInternal(contextIdentifier);

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


        #region Private

        private async Task<ContractMetadata?> GetSCMetadataInternal(string contextIdentifier)
        {
            string abiPath = _pathsLookup.ContextAbiPath(contextIdentifier);
            string bytecodePath = _pathsLookup.ContextBytecodePath(contextIdentifier);


            if (File.Exists(abiPath) && File.Exists(bytecodePath))
            {
                var abi = await File.ReadAllTextAsync(abiPath);
                var bytecode = await File.ReadAllTextAsync(bytecodePath);


                var contractMetadata = new ContractMetadata(bytecode.Trim('"'), abi);

                return contractMetadata;
            }

            return null;
        }

        #endregion

    }
}
