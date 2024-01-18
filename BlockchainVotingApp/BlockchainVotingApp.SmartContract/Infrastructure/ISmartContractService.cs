using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface ISmartContractService
    {
 
        Task<bool> AddCandidate(int candidateId, string contractAddress);
     
        Task<bool> Vote(Proof voterProof, int candidateId, string contractAddress);
     
        Task<bool> ChangeElectionState(bool electionState, string contractAddress);
     
        Task<Vote> GetUserVote(Proof voterProof, string contractAddress);
      
        Task<int> GetTotalNumberOfVotes(string contractAddress);
 
    }
}
