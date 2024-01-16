using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Microsoft.Extensions.Configuration;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using static BlockchainVotingApp.SmartContract.Infrastructure.ISmartContractService;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal class SmartContractService : ISmartContractService
    {
        private record ExecutionResult(string Message, bool IsSuccess);

        private readonly IConfiguration _configuration;
        private string _blockchainNetworkUrl;
        private string _adminDefaultAccountAddress;

        public SmartContractService(IConfiguration configuration)
        {
            _configuration = configuration;

            _blockchainNetworkUrl = _configuration.GetSection("SmartContract").GetSection("BlockchainNetworkUrl").Value;
            _adminDefaultAccountAddress = _configuration.GetSection("SmartContract").GetSection("AdminDefaultAccountAddress").Value;
        }


        public async Task<bool> AddCandidate(int candidateId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "addCandidate", candidateId)).IsSuccess;

        public async Task<bool> AddVoter(int voterId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "addVoter", voterId)).IsSuccess;

        public async Task<bool> AddVoters(List<int> votersIds, string contractAddress) => (await ExecuteSmartContract(contractAddress, "addVoters", votersIds)).IsSuccess;

        public async Task<bool> ChangeElectionState(bool electionState, string contractAddress) => (await ExecuteSmartContract(contractAddress, "changeElectionState", electionState)).IsSuccess;

        public async Task<string> DeploySmartContract(string adminAccountPrivateKey)
        {
            string adminDefaultAccountPrivateKey = adminAccountPrivateKey;

            if (string.IsNullOrEmpty(adminDefaultAccountPrivateKey))
            {
                adminDefaultAccountPrivateKey = _configuration.GetSection("SmartContract").GetSection("AdminDefaultAccountPrivateKey").Value;
            }

            var account = new Account(adminDefaultAccountPrivateKey);
            var web3 = new Web3(account, _blockchainNetworkUrl);

            ElectionDeployment deploymentMessage = new ElectionDeployment();

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

        public async Task<Votes> GetUserVote(int voterId, string contractAddress)
        {
            var web3 = new Web3(_blockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(ElectionDeployment.ABI, contractAddress);

                var result = await contract.GetFunction("getUserVote").CallDeserializingToObjectAsync<Votes>(voterId);

                return result;
            }
            catch
            {
                return new Votes();
            }
        }

        public async Task<int> GetTotalNumberOfVotes(string contractAddress)
        {
            var web3 = new Web3(_blockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(ElectionDeployment.ABI, contractAddress);

                var result = await contract.GetFunction("votesCount").CallAsync<int>();

                return result;
            }
            catch(RpcResponseException ex)
            {
                Console.WriteLine($" [RpcResponseException]. Message: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> HasUserVoted(int voterId, string contractAddress)
        {
            var web3 = new Web3(_blockchainNetworkUrl);

            var contract = web3.Eth.GetContract(ElectionDeployment.ABI, contractAddress);

            var result = await contract.GetFunction("checkUserHasVoted").CallAsync<bool>(voterId);

            return result;
        }

        public async Task<bool> Vote(int voterId, int candidateId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "castVote", voterId, candidateId)).IsSuccess;


        #region Internals

        private async Task<ExecutionResult> ExecuteSmartContract(string contractAddress, string functionName, params object[]? parameters)
        {
            var web3 = new Web3(_blockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(ElectionDeployment.ABI, contractAddress);

                var function = contract.GetFunction(functionName);

                // Estimate the gas required to execute the smart contract function.
                var estimatedGas = await EstimateGas(function);

                var result = await function.SendTransactionAsync(
                    from: _adminDefaultAccountAddress,
                    gas: estimatedGas,
                    value: new HexBigInteger(new BigInteger(0)),
                    parameters
                );

                return new ExecutionResult(result, true);
            }
            catch (RpcResponseException response)
            {
                return new ExecutionResult(response.RpcError.Message, false);
            }
            catch
            {
                return new ExecutionResult(string.Empty, false);
            }

            async Task<HexBigInteger> EstimateGas(Function function)
            {
                try
                {
                    return await function.EstimateGasAsync(parameters);
                }
                catch
                {
                    return new HexBigInteger(new BigInteger(50000));
                }
            }
        }

        #endregion
    }
}
