using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Http.Features;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionModule : IElectionModule
    {
        private readonly IElectionRepository _electionRepository;
        private readonly ISmartContractService _smartContractService;
        public ElectionModule(IElectionRepository electionRepository, ISmartContractService smartContractService)
        {
            _electionRepository = electionRepository;
            _smartContractService = smartContractService;
        }
        public async Task<bool> UpdateElectionState(int electionId)
        {
            var election = await _electionRepository.Get(electionId);

            bool result = true;

            if (election != null )
            {
                DateTime actualDateTime = DateTime.UtcNow;

                if (actualDateTime < election.StartDate)
                {
                    election.State = ElectionState.Upcoming;
                    result = await _smartContractService.ChangeElectionState(true, election.ContractAddress);
                }
                if (actualDateTime > election.EndDate)
                {
                    election.State = ElectionState.Completed;
                    result = await _smartContractService.ChangeElectionState(false, election.ContractAddress);
                }
                else
                {
                    election.State = ElectionState.Ongoing;
                    result = await _smartContractService.ChangeElectionState(false, election.ContractAddress);
                }
            }
            return result;
        }
    }
}
