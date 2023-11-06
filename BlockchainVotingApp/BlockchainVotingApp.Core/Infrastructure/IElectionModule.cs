using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{
    /// <summary>
    /// //This service is used in order to change the election state
    /// </summary>
    public interface IElectionModule
    {
        public Task<bool> UpdateElectionState(int electionId);
        public Task<bool> UpdateElectionsState();
    }
}
