using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Data.Repositories
{
    public interface ICandidateRepository
    {
        public Task<int> Insert(DbCandidate dbCandidate);
        public Task<int> Update(DbCandidate dbCandidate);
        public Task<List<DbCandidate>> GetAll();
        public Task<List<DbCandidate>> GetAllForElection(int electionId);
        public Task<DbCandidate?> Get(int id);
        public Task<bool> Delete(DbCandidate dbCandidate);
    }
}
