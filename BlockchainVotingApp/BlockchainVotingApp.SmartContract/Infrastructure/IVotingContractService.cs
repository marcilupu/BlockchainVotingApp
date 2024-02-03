using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public record ExecutionResult(bool IsSuccess, string? Message = null);
    
    public record ExecutionResult<ValueType>(ValueType? Value, bool IsSuccess, string? Message = null) : ExecutionResult(IsSuccess, Message);

    public interface IVotingContractService
    {
 
        Task<ExecutionResult> AddCandidate(int candidateId, string contractAddress);
     
        Task<ExecutionResult> Vote(VotingProof voterProof, int candidateId, string contractAddress);
     
        Task<ExecutionResult> ChangeElectionState(bool electionState, string contractAddress);
     
        Task<ExecutionResult<Vote>> GetUserVote(VotingProof voterProof, string contractAddress);
      
        Task<ExecutionResult<int>> GetTotalNumberOfVotes(string contractAddress);

        Task<ExecutionResult<CandidateResult>> GetCandidateResult(int candidateId, string contractAddress);


    }
}
