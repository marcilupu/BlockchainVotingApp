using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace BlockchainVotingApp.SmartContract.Services
{

    internal class SmartContractService : ISmartContractService
    {

        private readonly ISmartContractConfiguration _configuration;
        private readonly ContractMetadata _metadata;

        public SmartContractService(ContractMetadata metadata, ISmartContractConfiguration configuration)
        {
            _configuration = configuration;
            _metadata = metadata;

        }

        public async Task<ExecutionResult> AddCandidate(int candidateId, string contractAddress) => (await ExecuteSmartContract(contractAddress, "addCandidate", candidateId));


        public async Task<ExecutionResult> ChangeElectionState(bool electionState, string contractAddress) => (await ExecuteSmartContract(contractAddress, "changeElectionState", electionState));

        public async Task<ExecutionResult<Vote>> GetUserVote(Proof voterProof, string contractAddress)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var proofHash = voterProof.GetHash();

                var result = await contract.GetFunction("getUserVote").CallDeserializingToObjectAsync<Vote>(proofHash);

                return new ExecutionResult<Vote>(result, false);
            }
            catch
            {
                return new ExecutionResult<Vote>(new Vote(), false);
            }
        }

        public async Task<ExecutionResult<int>> GetTotalNumberOfVotes(string contractAddress)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var result = await contract.GetFunction("votesCount").CallAsync<int>();

                return new ExecutionResult<int>(result, true);
            }
            catch (RpcResponseException ex)
            {
                Console.WriteLine($" [RpcResponseException]. Message: {ex.Message}");

                return new ExecutionResult<int>(-1, false, ex.Message);
            }
        }


        public async Task<ExecutionResult> Vote(Proof voterProof, int candidateId, string contractAddress)
        {
            var proofHash = voterProof.GetHash();

            return (await ExecuteSmartContract(contractAddress, "castVote",
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
