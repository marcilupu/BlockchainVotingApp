using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface ISmartContractService
    {
        [FunctionOutput]
        public class Votes : IFunctionOutputDTO
        {
            [Parameter("int", "VoterId", 1)]
            public int VoterId { get; set; }

            [Parameter("int", "CandidateId", 2)]
            public int CandidateId { get; set; }
        }

        public Task<string> DeploySmartContract(string adminAccountPrivateKey);

        public Task<bool> AddCandidate(int candidateId, string contractAddress);
        public Task<bool> AddVoters(List<int> votersIds, string contractAddress);
        public Task<bool> Vote(int voterId, int candidateId, string contractAddress);
        public Task<bool> HasUserVoted(int voterId, string contractAddress);
        public Task<bool> ChangeElectionState(bool electionState, string contractAddress);
        public Task<Votes> GetUserVote(int voterId, string contractAddress);
    }
}
