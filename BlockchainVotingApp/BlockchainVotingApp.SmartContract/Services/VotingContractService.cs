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

    internal class VotingContractService : ElectionContractService, IVotingContractService
    {
        public VotingContractService(ContractMetadata metadata, IContractConfiguration configuration) : base(metadata, configuration) { }

        public async Task<ExecutionResult> AddCandidate(int candidateId, string contractAddress) => (await Post(contractAddress, "addCandidate", candidateId));


        public async Task<ExecutionResult> ChangeElectionState(bool electionState, string contractAddress) => (await Post(contractAddress, "changeElectionState", electionState));

        public async Task<ExecutionResult<Vote>> GetUserVote(Proof voterProof, string contractAddress)
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

        public async Task<ExecutionResult> Vote(Proof voterProof, int candidateId, string contractAddress)
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
    }
}
