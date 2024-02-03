using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using System.Numerics;

namespace BlockchainVotingApp.SmartContract.Services
{

    internal class VotingContractService : IVotingContractService
    {

        private readonly IContractConfiguration _configuration;
        private readonly ContractMetadata _metadata;

        public VotingContractService(ContractMetadata metadata, IContractConfiguration configuration)
        {
            _configuration = configuration;
            _metadata = metadata;

        }

        public async Task<ExecutionResult> AddCandidate(int candidateId, string contractAddress) => (await Post(contractAddress, "addCandidate", candidateId));


        public async Task<ExecutionResult> ChangeElectionState(bool electionState, string contractAddress) => (await Post(contractAddress, "changeElectionState", electionState));

        public async Task<ExecutionResult<Vote>> GetUserVote(VotingProof voterProof, string contractAddress)
        {
            var proofHash = voterProof.GetHash();

            return await Get(contractAddress, "getUserVote", function => function.CallDeserializingToObjectAsync<Vote>(proofHash), defaultValue: new());
        }

        public async Task<ExecutionResult<int>> GetTotalNumberOfVotes(string contractAddress)
        {
            return await Get(contractAddress, "votesCount", function => function.CallAsync<int>(), defaultValue: -1);
        }

        public async Task<ExecutionResult<CandidateResult>> GetCandidateResult(int candidateId, string contractAddress)
        {
            return await Get(contractAddress, "getCandidateResults", function => function.CallDeserializingToObjectAsync<CandidateResult>(candidateId), defaultValue: new());
        }

        public async Task<ExecutionResult> Vote(VotingProof voterProof, int candidateId, string contractAddress)
        {
            var proofHash = voterProof.GetHash();

            return (await Post(contractAddress, "castVote",
                voterProof.AX,
                voterProof.AY,
                voterProof.B0X,
                voterProof.B1X,
                voterProof.B0Y,
                voterProof.B1Y,
                voterProof.CX,
                voterProof.CY,
                proofHash,
                candidateId));
        }

        #region Internals

        private async Task<ExecutionResult<DataType>> Get<DataType>(string contractAddress, string functionName, Func<Function, Task<DataType>> executor, DataType? defaultValue = default)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var function = contract.GetFunction(functionName);

                var result = await executor(function);

                return new ExecutionResult<DataType>(result, true);
            }
            catch (RpcResponseException ex)
            {
                Console.WriteLine($" [RpcResponseException]. Message: {ex.Message}");

                return new ExecutionResult<DataType>(defaultValue, false, ex.Message);
            }
            catch
            {
                return new ExecutionResult<DataType>(defaultValue, false);
            }
        }

        private async Task<ExecutionResult> Post(string contractAddress, string functionName, params object[]? parameters)
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

                return new ExecutionResult(true, result);
            }
            catch (RpcResponseException response)
            {
                return new ExecutionResult(false, response.RpcError.Message.GetErrorMessage());
            }
            catch
            {
                return new ExecutionResult(false);
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
