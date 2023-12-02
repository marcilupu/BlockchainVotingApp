using BlockchainVotingApp.AppCode.Utilities;
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
                return await RetrieveInternal(dbElection, null);
            }

            return null;
        }

        public async Task<UserElection?> GetElectionForUser(int id, int userId)
        {
            var dbElection = await _electionRepository.Get(id);

            if (dbElection != null)
            {
                return await RetrieveInternal(dbElection, userId);
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

            var retrieveTasks = electionsRepo.Select(async dbElection => await RetrieveInternal(dbElection, null)).ToList();

            var elections = (await Task.WhenAll(retrieveTasks)).ToList();

            foreach (var election in elections)
            {
                election.HasVoted = await _smartContractService.HasUserVoted(user.Id, election.ContractAddress);
            }

            return elections;
        }

        public async Task<int> GetVoteForUser(int userId, string contractAddress)
        {
            var result = await _smartContractService.GetUserVote(userId, contractAddress);
            if (result != null)
            {
                return result.CandidateId;
            }

            return 0;
        }

        public async Task<List<UserElection>> GetVotesForUser(AppUser user)
        {
            var elections = await GetAllByCounty(user);

            foreach (var election in elections)
            {
                var candidateId = await GetVoteForUser(user.Id, election.ContractAddress);

                var candidate = await _candidateService.Get(candidateId);

                if (candidate != null)
                {
                    election.SelectedCandidate = candidate.FullName;
                }

                election.NumberOfVotes = await ElectionHelper.GetElectionVotes(_smartContractService, election.ContractAddress);
            }

            return elections.Where(x => x.HasVoted).ToList();
        }

        public async Task<int> Insert(DbElection election)
        {
            //Deploy a new smart contract to interact with
            var deployedContract = await _smartContractService.DeploySmartContract(string.Empty);
            election.ContractAddress = deployedContract;

            int electionId = await _electionRepository.Insert(election);

            var result = await AddVoters(election);

            if(!result)
            {
                return 0;
            }

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

        private async Task<bool> AddVoters(DbElection election)
        {
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

            if (result)
            {
                return true;
            }

            return false;
        }

        #region Private

        private async Task<UserElection> RetrieveInternal(DbElection dbElection, int? userId)
        {
            var election = new UserElection(dbElection);

            if (userId.HasValue)
            {
                election.HasVoted = await _smartContractService.HasUserVoted(userId.Value, election.ContractAddress);
            }
            else
            {
                election.HasVoted = false;
            }

            election.NumberOfVotes = await ElectionHelper.GetElectionVotes(_smartContractService, election.ContractAddress);

            return election;
        }

        #endregion
    }
}
