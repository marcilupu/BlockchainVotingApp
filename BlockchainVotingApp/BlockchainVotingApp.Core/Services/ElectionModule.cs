﻿using BlockchainVotingApp.AppCode.Utilities;
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
        private readonly IVotingContractGenerator _smartContractGenerator;
        private readonly IVotingContractServiceFactory _smartContractServiceFactory;

        public ElectionModule(IElectionRepository electionRepository, IVotingContractServiceFactory smartContractServiceFactory, IVotingContractGenerator smartContractGenerator)
        {
            _electionRepository = electionRepository;
            _smartContractServiceFactory = smartContractServiceFactory;
            _smartContractGenerator = smartContractGenerator;
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
                string contextIdentifier = ElectionHelper.CreateContextIdentifier(electionId, election.Name);

                var contractMetadata = await _smartContractGenerator.GetSmartContractMetadata(contextIdentifier);

                var smartContractService = _smartContractServiceFactory.Create(contractMetadata);

                DateTime actualDateTime = DateTime.UtcNow;

                if (actualDateTime < election.StartDate)
                {
                    election.State = ElectionState.Upcoming;
                    await _electionRepository.Update(election);
                    return (await smartContractService.ChangeElectionState(true, election.ContractAddress)).IsSuccess;
                }
                if (actualDateTime > election.EndDate)
                {
                    election.State = ElectionState.Completed;
                    await _electionRepository.Update(election);
                    return (await smartContractService.ChangeElectionState(false, election.ContractAddress)).IsSuccess;
                }
                else
                {
                    election.State = ElectionState.Ongoing;
                    await _electionRepository.Update(election);
                    return (await smartContractService.ChangeElectionState(false, election.ContractAddress)).IsSuccess;
                }
            }
            return false;
        }
    }
}
