using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionService : IElectionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserVoteRepository _userVoteRepository;

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
                               ISmartContractConfiguration smartContractConfiguration,
                               IUserVoteRepository userVoteRepository)
        {
            _electionRepository = electionRepository;
            _userRepository = userRepository;
            _smartContractServiceFactory = smartContractServiceFactory;
            _candidateService = candidateService;
            _candidateRepository = candidateRepository;
            _smartContractGenerator = smartContractGenerator;
            _smartContractConfiguration = smartContractConfiguration;
            _userVoteRepository = userVoteRepository;
        }

        public async Task<UserElection?> GetUserElection(int id, AppUser user)
        {
            var dbElection = await _electionRepository.Get(id);

            if (dbElection != null)
            {
                // Check if user has voted or not
                var userVote = await _userVoteRepository.Get(user.Id, id);

                // Build the domain model.
                var election = await GetElectionInternal(dbElection, userVote);

                return election;
            }

            return null;
        }

        public async Task<List<Election>> GetAll()
        {
            var dbElections = await _electionRepository.GetAll();

            var elections = dbElections.Select(item => new Election(item)).ToList();

            return elections;
        }

        public async Task<List<UserElection>> GetAll(AppUser user)
        {
            // Retrieve all elections where user have access to vote into.
            var elections = await _electionRepository.GetAllByCounty(user.CountyId);

            // Get all user votes and map them so the UserElection domain model may be constructed.
            var userVotes = (await _userVoteRepository.GetAll(user.Id)).ToDictionary(v => v.ElectionId, v => v);


            var retrieveTasks = elections.Select(async dbElection => await GetElectionInternal(dbElection, dbVote: userVotes.TryGetValue(dbElection.Id, out var vote) ? vote : null)).ToList();

            return (await Task.WhenAll(retrieveTasks)).ToList();
        }

        //TODO: fix it
        public async Task<int?> GetUserVote(AppUser user, string proof, int electionId)
        {
            var election = await GetUserElection(electionId, user);

            if (election != null)
            {
                ISmartContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                if (smartContractService != null)
                {
                    Proof voterProof = JsonConvert.DeserializeObject<Proof>(proof);

                    var result = await smartContractService.GetUserVote(voterProof, election.ContractAddress);

                    if (result != null && result.Value != null)
                    {
                        return result.Value.CandidateId;
                    }
                }
            }
            return -1;
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

            ISmartContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);


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

                if (currentElection.State != newState)
                {
                    if (!string.IsNullOrEmpty(currentElection.ContractAddress))
                    {
                        bool isUpcoming = newState == ElectionState.Upcoming;

                        var executionResult = await smartContractService.ChangeElectionState(isUpcoming, currentElection.ContractAddress);

                        return executionResult.IsSuccess;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
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
                var election = await GetUserElection(candidate.ElectionId, user);

                if (election != null)
                {
                    ISmartContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                    if (smartContractService != null)
                    {
                        var result = await smartContractService.Vote(voterProof, candidateId, election.ContractAddress);

                        // Insert a new entry in the UserVotes table to have an evidence is the user has voted or not.
                        if (result.IsSuccess)
                        {
                            var dbUserVote = new DbUserVote();
                            dbUserVote.UserId = user.Id;
                            dbUserVote.ElectionId = election.Id;
                            dbUserVote.HasVoted = true;

                            await _userVoteRepository.Insert(dbUserVote);
                        }

                        return new VoteResult(result.IsSuccess, result.Message);
                    }
                    return new VoteResult(false, "The smart contract service is null");
                }
            }
            return new VoteResult(false, "The candidate is null");
        }

        #region Private


        private async Task<UserElection> GetElectionInternal(DbElection dbElection, DbUserVote? dbVote)
        {
            var election = new UserElection(dbElection)
            {
                HasVoted = dbVote?.HasVoted ?? false
            };

            ISmartContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

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
