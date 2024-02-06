using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{

    public record VoteResult(bool Success, string? ErrorMessage);

    public interface IElectionService
    {
        Task<int> Insert(DbElection election, int? countyId);
        
        Task<int> Update(DbElection election);

        Task<bool> Delete(DbElection election);

        Task<List<UserElection>> GetAll(AppUser user);

        Task<List<Election>> GetAll();

        Task<UserElection?> GetUserElection(int id, AppUser user);

        Task<VoteResult> Vote(AppUser user, string proof, int candidateId);

        Task<int?> GetUserVote(AppUser user, string proof, int electionId);

        Task<bool> InitializeElectionContext(DbElection election);

        Task<bool> ChangeElectionState(DbElection currentElection, ElectionState newState);

        Task<int> GetElectionResult(DbElection election);

        Task<List<UserElection>> GetRegisteredElections(AppUser user);
    }
}
