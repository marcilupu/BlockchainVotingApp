using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IElectionService
    {
        Task<List<UserElection>> GetAllByCounty(int countyId);

        Task<UserElection?> Get(int id);
    }
}
