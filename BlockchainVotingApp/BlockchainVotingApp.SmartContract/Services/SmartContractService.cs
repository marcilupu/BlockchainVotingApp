using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models; 
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using System.Numerics;

namespace BlockchainVotingApp.SmartContract.Services
{

    internal class SmartContractService : ISmartContractService
    {
        private record ExecutionResult(string Message, bool IsSuccess);

        private readonly ISmartContractConfiguration _configuration;
        private readonly ContractMetadata _metadata;
 
        public SmartContractService(ContractMetadata metadata, ISmartContractConfiguration configuration)
        {
            _configuration = configuration;
            _metadata = metadata;

        }

        public async Task<bool> AddCandidate(int candidateId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "addCandidate", candidateId)).IsSuccess;


        public async Task<bool> ChangeElectionState(bool electionState, string contractAddress) => (await ExecuteSmartContract(contractAddress, "changeElectionState", electionState)).IsSuccess;

        //TODO: fix it
        public async Task<Vote> GetUserVote(Proof voterProof, string contractAddress)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                //var result = await contract.GetFunction("getUserVote").CallDeserializingToObjectAsync<Votes>(voterId);

                //return result;

                return null;
            }
            catch
            {
                return new Vote();
            }
        }

        public async Task<int> GetTotalNumberOfVotes(string contractAddress)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var result = await contract.GetFunction("votesCount").CallAsync<int>();

                return result;
            }
            catch (RpcResponseException ex)
            {
                Console.WriteLine($" [RpcResponseException]. Message: {ex.Message}");
                return 0;
            }
        }

        //TODO: fix it if necessary
        public async Task<bool> HasUserVoted(int voterId, string contractAddress)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

            var result = await contract.GetFunction("checkUserHasVoted").CallAsync<bool>(voterId);

            return result;
        }

        public async Task<bool> Vote(Proof voterProof,  int candidateId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "castVote", 
            voterProof.AX, 
            voterProof.AY, 
            voterProof.B0X, 
            voterProof.B0Y,
            voterProof.B1X, 
            voterProof.B1Y, 
            voterProof.CX, 
            voterProof.CY, candidateId)).IsSuccess;


        #region Internals

        private async Task<ExecutionResult> ExecuteSmartContract(string contractAddress, string functionName, params object[]? parameters)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var function = contract.GetFunction(functionName);
                
                // Estimate the gas required to execute the smart contract function.
                var estimatedGas = await EstimateGas(function);

                var result = await function.SendTransactionAsync(
                    from: _configuration.AdminDefaultAccountAddress,
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
