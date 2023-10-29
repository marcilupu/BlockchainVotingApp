using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionService : IElectionService
    {
        private readonly IElectionRepository _electionRepository;


        public ElectionService(IElectionRepository electionRepository)
        {
            _electionRepository = electionRepository;
        }

        public async Task<UserElection?> Get(int id)
        {
            var dbElection = await _electionRepository.Get(id);

            if (dbElection != null)
            {
                return await RetrieveInternal(dbElection);
            }

            return null;
        }

        public async Task<List<UserElection>> GetAllByCounty(int countyId)
        {
            var electionsRepo = await _electionRepository.GetAllByCounty(countyId);

            var retrieveTasks = electionsRepo.Select(async dbElection => await RetrieveInternal(dbElection)).ToList();

            return (await Task.WhenAll(retrieveTasks)).ToList();
        }


        #region Private

        private async Task<UserElection> RetrieveInternal(DbElection dbElection)
        {
            var election = new UserElection(dbElection);

            election.HasVoted = true;

            return election;
        }

        #endregion
    }
}
