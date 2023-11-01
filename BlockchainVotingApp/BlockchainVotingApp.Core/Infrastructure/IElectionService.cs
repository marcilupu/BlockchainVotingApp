using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IElectionService
    {
        public Task<int> Insert(DbElection election);
        public Task<int> Update(DbElection election);
        Task<List<UserElection>> GetAllByCounty(AppUser user);

        public Task<List<Election>> GetAll();

        Task<UserElection?> Get(int id);

        public Task<bool> Vote(int userId, int candidateId);
    }
}
