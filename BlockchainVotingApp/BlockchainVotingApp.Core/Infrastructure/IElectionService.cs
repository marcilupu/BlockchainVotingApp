using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IElectionService
    {
        Task<int> Insert(DbElection election);
        
        Task<int> Update(DbElection election);

        Task<List<UserElection>> GetAllByCounty(AppUser user);

        Task<List<Election>> GetAll();

        Task<UserElection?> Get(int id);

        Task<UserElection?> GetElectionForUser(int id, int userId);

        Task<bool> Vote(int userId, int candidateId);

        Task<int> GetVoteForUser(int userId, string ContractAddress);

        Task<List<UserElection>> GetVotesForUser(AppUser user);

        Task<bool> GenerateElectionSmartContract(DbElection election);
    }
}
