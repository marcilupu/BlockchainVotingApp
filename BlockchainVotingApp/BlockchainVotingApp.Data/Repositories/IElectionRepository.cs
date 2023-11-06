using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Data.Repositories
{
    public interface IElectionRepository
    {
        public Task<int> Insert(DbElection election);
        public Task<int> Update(DbElection election);

        public Task<DbElection?> Get(int electionId);
        public Task<List<DbElection>> GetAll();

        public Task<List<DbElection>> GetAllByCounty(int countyId);

        public Task<bool> Delete(DbElection election);
    }
}
