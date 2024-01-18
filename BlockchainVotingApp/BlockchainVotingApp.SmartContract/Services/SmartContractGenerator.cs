using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
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


        public Task<ContractMetadata> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> userIds)
        {
            throw new NotImplementedException();
        }

        public Task GenerateProof(string contextIdentifier, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ContractMetadata?> GetSmartContractContext(string contextIdentifier)
        {
            throw new NotImplementedException();
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

            ElectionDeployment deploymentMessage = new ElectionDeployment(""/*todo: obtine abi*/);

            try
            {
                //var estimatedGas = await web3.Eth.Transactions.EstimateGas.SendRequestAsync(new Nethereum.RPC.Eth.DTOs.CallInput
                //{
                //    From = _adminDefaultAccountAddress,
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

   
    }
}
