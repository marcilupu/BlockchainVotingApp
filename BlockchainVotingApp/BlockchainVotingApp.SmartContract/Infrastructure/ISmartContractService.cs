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

        Task<string> DeploySmartContract(string adminAccountPrivateKey);

        Task<bool> AddCandidate(int candidateId, string contractAddress);
        Task<bool> AddVoter(int voterId, string contractAddress);
        Task<bool> AddVoters(List<int> votersIds, string contractAddress);
        Task<bool> Vote(int voterId, int candidateId, string contractAddress);
        Task<bool> HasUserVoted(int voterId, string contractAddress);
        Task<bool> ChangeElectionState(bool electionState, string contractAddress);
        Task<Votes> GetUserVote(int voterId, string contractAddress);
    }
}
