using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal abstract class ElectionContractGenerator : IElectionContractGenerator
    {
        protected readonly IContractConfiguration _configuration;
        protected readonly PathsLookup _pathsLookup;

        public ElectionContractGenerator(IContractConfiguration configuration, PathsLookup pathsLookup)
        {
            _configuration = configuration;
            _pathsLookup = pathsLookup;
        }

        protected abstract string Type { get; }

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
                ContractDeployment deploymentMessage = new ContractDeployment(contextMetadata.Bytecode);

                try
                {
                    //var estimatedgas = await web3.Eth.Transactions.EstimateGas.SendRequestAsync(new Nethereum.RPC.Eth.DTOs.CallInput
                    //{
                    //    From = _configuration.AdminDefaultAccountAddress,
                    //    Data = deploymentMessage.ByteCode
                    //});

                    var deploymentHandler = web3.Eth.GetContractDeploymentHandler<ContractDeployment>();
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

        protected async Task<ContractMetadata?> GetSCMetadataInternal(string contextIdentifier)
        {
            string abiPath = _pathsLookup.ContextAbiPath(contextIdentifier, Type);

            string bytecodePath = _pathsLookup.ContextBytecodePath(contextIdentifier, Type);


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
