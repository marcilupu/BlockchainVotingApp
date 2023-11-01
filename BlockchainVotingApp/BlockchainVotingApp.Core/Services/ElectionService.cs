using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using System.ComponentModel.Design;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionService : IElectionService
    {
        private readonly IElectionRepository _electionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISmartContractService _smartContractService;
        private readonly ICandidateService _candidateService;

        public ElectionService(IElectionRepository electionRepository,
                               IUserRepository userRepository,
                               ISmartContractService smartContractService,
                               ICandidateService candidateService)
        {
            _electionRepository = electionRepository;
            _userRepository = userRepository;
            _smartContractService = smartContractService;
            _candidateService = candidateService;
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

        public async Task<List<Election>> GetAll()
        {
            var dbElections = await _electionRepository.GetAll();
            var elections = dbElections.Select(item =>
            {
                return new Election(item);
            }).ToList();

            return elections;
        }

        public async Task<List<UserElection>> GetAllByCounty(AppUser user)
        {
            var electionsRepo = await _electionRepository.GetAllByCounty(user.CountyId);

            var retrieveTasks = electionsRepo.Select(async dbElection => await RetrieveInternal(dbElection)).ToList();

            var elections = (await Task.WhenAll(retrieveTasks)).ToList();

            foreach (var election in elections)
            {
                var hasVoted = await _smartContractService.HasUserVoted(user.Id, election.ContractAddress);

                if (hasVoted)
                    election.HasVoted = true;
            }

            return elections;
        }

        public async Task<int> Insert(DbElection election)
        {
            //Deploy a new smart contract to interact with
            var deployedContract = await _smartContractService.DeploySmartContract(string.Empty);
            election.ContractAddress = deployedContract;

            int electionId = await _electionRepository.Insert(election);

            //Get all the voters that can vote for the current election
            //If the election is intended for a specific country/administrative-territorial unit, only the users who resides in that particular area are allowed to vote
            List<DbUser> users;
            if (election.CountyId.HasValue)
            {
                users = await _userRepository.GetAllByCounty(election.CountyId.Value);
            }
            else
            {
                users = await _userRepository.GetAll();
            }

            List<int> usersIds = users.Select(item => item.Id).ToList();

            //Storage into the smart contract the users that cand vote for this election
            var result = await _smartContractService.AddVoters(usersIds, election.ContractAddress);

            return electionId;
        }

        public async Task<int> Update(DbElection election)
        {
            return await _electionRepository.Update(election);
        }

        public async Task<bool> Vote(int userId, int candidateId)
        {
            var candidate = await _candidateService.Get(candidateId);

            if (candidate != null)
            {
                var election = await Get(candidate.ElectionId);

                if (election != null)
                {
                    var result = await _smartContractService.Vote(userId, candidateId, election.ContractAddress);

                    return result;
                }
            }

            return false;
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
