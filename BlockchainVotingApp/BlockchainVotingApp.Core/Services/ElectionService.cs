using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionService : IElectionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateRepository _candidateRepository;

        private readonly ICandidateService _candidateService;
        private readonly ISmartContractServiceFactory _smartContractServiceFactory;
        private readonly ISmartContractGenerator _smartContractGenerator;

        private readonly ISmartContractConfiguration _smartContractConfiguration;

        public ElectionService(IElectionRepository electionRepository,
                               IUserRepository userRepository,
                               ISmartContractServiceFactory smartContractServiceFactory,
                               ICandidateService candidateService,
                               ICandidateRepository candidateRepository,
                               ISmartContractGenerator smartContractGenerator,
                               ISmartContractConfiguration smartContractConfiguration)
        {
            _electionRepository = electionRepository;
            _userRepository = userRepository;
            _smartContractServiceFactory = smartContractServiceFactory;
            _candidateService = candidateService;
            _candidateRepository = candidateRepository;
            _smartContractGenerator = smartContractGenerator;
            _smartContractConfiguration = smartContractConfiguration;
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

        public async Task<UserElection?> GetUserElection(int id, AppUser user)
        {
            var dbElection = await _electionRepository.Get(id);

            if (dbElection != null)
            {
                return await RetrieveInternal(dbElection, user.Id);
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

        public async Task<List<UserElection>> GetAll(AppUser user)
        {
            var electionsRepo = await _electionRepository.GetAllByCounty(user.CountyId);

            var retrieveTasks = electionsRepo.Select(async dbElection => await RetrieveInternal(dbElection, null)).ToList();

            var elections = (await Task.WhenAll(retrieveTasks)).ToList();

            foreach (var election in elections)
            {
                election.HasVoted = user.HasVoted;
            }

            return elections;
        }

        //TODO: fix it
        public async Task<int?> GetUserVote(AppUser user, string proof, int electionId)
        {
            //todo: REFACTOR
            var result = (Vote?)null; // await _smartContractService.GetUserVote(userId, contractAddress);

            if (result != null)
            {
                return result.CandidateId;
            }

            return 0;
        }
         

        public async Task<bool> InitializeElectionContext(DbElection election)
        {
            // Get users for this election
            var usersIds = await GetUserIds(election);

            // Get election context name
            string contextIdentifier = ElectionHelper.CreateContextIdentifier(election.Id, election.Name);

            // Generate election context
            var contractMetadata = await _smartContractGenerator.CreateSmartContractContext(contextIdentifier, usersIds);

            //Deploy a new smart contract to interact with
            var deployedContract = await _smartContractGenerator.DeploySmartContract(contextIdentifier, _smartContractConfiguration.AdminDefaultAccountPrivateKey);
            election.ContractAddress = deployedContract;

            await Update(election);

            ISmartContractService smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

            //Add candidates to smart contract
            var candidates = await _candidateRepository.GetAllForElection(election.Id);
            foreach (var candidate in candidates)
            {
                var candidateResult = await smartContractService.AddCandidate(candidate.Id, election.ContractAddress);

                //If the smart contract add candidate failed, drop the candidate from the db
                if (!candidateResult.IsSuccess)
                {
                    await _candidateRepository.Delete(candidate);
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> ChangeElectionState(DbElection currentElection, ElectionState newState)
        {
            ISmartContractService smartContractService;
            var smartContratMetadata = await _smartContractGenerator.GetSmartContractMetadata(ElectionHelper.CreateContextIdentifier(currentElection.Id, currentElection.Name));
            if (smartContratMetadata != null)
            {
                smartContractService = _smartContractServiceFactory.Create(smartContratMetadata);
                if (currentElection.State != newState && newState == ElectionState.Upcoming)
                {
                    await smartContractService.ChangeElectionState(true, currentElection.ContractAddress);
                }
                if (currentElection.State != newState && newState != ElectionState.Upcoming)
                {
                    await smartContractService.ChangeElectionState(false, currentElection.ContractAddress);
                }
            }

            return true;
        }

        public async Task<int> Insert(DbElection election)
        {
            int electionId = await _electionRepository.Insert(election);

            return electionId;
        }

        public async Task<int> Update(DbElection election)
        {
            return await _electionRepository.Update(election);
        }

        public async Task<VoteResult> Vote(AppUser user, string proof, int candidateId)
        {
            var candidate = await _candidateService.Get(candidateId);
           
            Proof voterProof = JsonConvert.DeserializeObject<Proof>(proof);
             
            if (candidate != null)
            {
                var election = await Get(candidate.ElectionId);

                if (election != null)
                { 
                    ISmartContractService smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);
                   
                    var result = await smartContractService.Vote(voterProof, candidateId, election.ContractAddress);

                    return new VoteResult(result.IsSuccess, result.Message);
                }
            }

            return new VoteResult(false, "The candidate is null");
        }

        #region Private
        
        private async Task<UserElection> RetrieveInternal(DbElection dbElection, int? userId)
        {
            var election = new UserElection(dbElection);

            ISmartContractService smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

            if (userId.HasValue)
            {
                election.HasVoted = false;//await smartContractService.HasUserVoted(userId.Value, election.ContractAddress);
            }
            else
            {
                election.HasVoted = false;
            }

            if (smartContractService != null)
            {
                election.NumberOfVotes = (await smartContractService.GetTotalNumberOfVotes(election.ContractAddress)).Value;
            }
            else
            {
                election.NumberOfVotes = 0;
            }

            return election;
        }

        private async Task<List<int>> GetUserIds(DbElection election)
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

            return usersIds;
        }

        #endregion
    }
}
