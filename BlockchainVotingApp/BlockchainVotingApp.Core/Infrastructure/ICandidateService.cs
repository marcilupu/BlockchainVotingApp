using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface ICandidateService
    {
        public Task<int> Insert(DbCandidate dbCandidate);
        public Task<int> Update(DbCandidate dbCandidate);
        public Task<List<Candidate>> GetAll();
        public Task<List<Candidate>> GetAllForElection(int electionId);
        public Task<Candidate?> Get(int id);
    }
}
