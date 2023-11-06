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

        public async Task<bool> UpdateElectionsState()
        {
            var elections = await _electionRepository.GetAll();

            foreach(var election in elections) {
                var result = await UpdateElectionState(election.Id);
            }

            return true;
        }

        public async Task<bool> UpdateElectionState(int electionId)
        {
            var election = await _electionRepository.Get(electionId);

            if (election != null )
            {
                DateTime actualDateTime = DateTime.UtcNow;

                if (actualDateTime < election.StartDate)
                {
                    election.State = ElectionState.Upcoming;
                    await _electionRepository.Update(election);
                    return await _smartContractService.ChangeElectionState(true, election.ContractAddress);
                }
                if (actualDateTime > election.EndDate)
                {
                    election.State = ElectionState.Completed;
                    await _electionRepository.Update(election);
                    return await _smartContractService.ChangeElectionState(false, election.ContractAddress);
                }
                else
                {
                    election.State = ElectionState.Ongoing;
                    await _electionRepository.Update(election);
                    return await _smartContractService.ChangeElectionState(false, election.ContractAddress);
                }
            }
            return false;
        }
    }
}
