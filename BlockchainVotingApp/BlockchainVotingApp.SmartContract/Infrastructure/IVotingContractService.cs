using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IVotingContractService : IElectionContractService
    {
        Task<ExecutionResult> AddCandidate(int candidateId, string contractAddress);
     
        Task<ExecutionResult> Vote(Proof voterProof, int candidateId, string contractAddress);
     
        Task<ExecutionResult> ChangeElectionState(bool electionState, string contractAddress);
     
        Task<ExecutionResult<Vote>> GetUserVote(Proof voterProof, string contractAddress);
      
        Task<ExecutionResult<int>> GetTotalNumberOfVotes(string contractAddress);

        Task<ExecutionResult<CandidateResult>> GetCandidateResult(int candidateId, string contractAddress);
    }
}
