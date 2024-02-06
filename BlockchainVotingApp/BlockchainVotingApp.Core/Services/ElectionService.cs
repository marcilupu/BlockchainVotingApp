using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Newtonsoft.Json;

namespace BlockchainVotingApp.Core.Services
{
    internal class ElectionService : IElectionService
    {
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserVoteRepository _userVoteRepository;

        private readonly ICandidateService _candidateService;
        private readonly IVotingContractGenerator _smartContractGenerator;
        private readonly IVotingContractServiceFactory _smartContractServiceFactory;

        private readonly IRegisterContractGenerator _registerContractGenerator;
        private readonly IRegisterContractServiceFactory _registerContractServiceFactory;

        private readonly IContractConfiguration _smartContractConfiguration;

        public ElectionService(IElectionRepository electionRepository,
                               IVotingContractServiceFactory smartContractServiceFactory,
                               ICandidateService candidateService,
                               ICandidateRepository candidateRepository,
                               IVotingContractGenerator smartContractGenerator,
                               IContractConfiguration smartContractConfiguration,
                               IUserVoteRepository userVoteRepository,
                               IRegisterContractGenerator registerContractGenerator,
                               IRegisterContractServiceFactory registerContractServiceFactory)
        {
            _electionRepository = electionRepository;
            _smartContractServiceFactory = smartContractServiceFactory;
            _candidateService = candidateService;
            _candidateRepository = candidateRepository;
            _smartContractGenerator = smartContractGenerator;
            _smartContractConfiguration = smartContractConfiguration;
            _userVoteRepository = userVoteRepository;
            _registerContractGenerator = registerContractGenerator;
            _registerContractServiceFactory = registerContractServiceFactory;
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
            var elections = await _electionRepository.GetAll();

            // Get all user votes and map them so the UserElection domain model may be constructed.
            var userVotes = (await _userVoteRepository.GetAll(user.Id)).ToDictionary(v => v.ElectionId, v => v);

            var retrieveTasks = elections.Select(async dbElection => await GetElectionInternal(dbElection, dbVote: userVotes.TryGetValue(dbElection.Id, out var vote) ? vote : null)).ToList();

            return (await Task.WhenAll(retrieveTasks)).ToList();
        }

        public async Task<List<UserElection>> GetRegisteredElections(AppUser user)
        {
            // Get all user votes and map them so the UserElection domain model may be constructed.
            var userVotes = (await _userVoteRepository.GetAll(user.Id, includeElection: true)).ToDictionary(v => v.ElectionId, v => v);

            List<UserElection> userElections = new List<UserElection>(userVotes.Count);

            foreach (var userVote in userVotes)
            {
                var election = await GetElectionInternal(userVote.Value.Election, userVote.Value);

                userElections.Add(election);
            }

            return userElections;
        }

        public async Task<int?> GetUserVote(AppUser user, string proof, int electionId)
        {
            var election = await GetUserElection(electionId, user);

            if (election != null)
            {
                IVotingContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

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

            return null;
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

            IVotingContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

            if (smartContractService != null)
            {
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
                election.State = ElectionState.Ongoing;

                var stateResult = await smartContractService.ChangeElectionState(false, election.ContractAddress);

                if (stateResult.IsSuccess)
                {
                    await Update(election);

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> ChangeElectionState(DbElection currentElection, ElectionState newState)
        {
            IVotingContractService smartContractService;

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

        public async Task<int> Insert(DbElection election, int? countyId)
        {
            int electionId = await _electionRepository.Insert(election);

            // Create a new registration context and deploy the created smart contracts
            var contextIdentifier = ElectionHelper.CreateContextIdentifier(electionId, election.Name);

            var contextResult = await _registerContractGenerator.CreateSmartContractContext(contextIdentifier, countyId);

            // If smart contract has been succesufully generated, continue.
            if (contextResult != null)
            {
                // If smart contract has been succesufully deployed, continue.
                var deployResult = await _registerContractGenerator.DeploySmartContract(contextIdentifier, _smartContractConfiguration.AdminDefaultAccountPrivateKey);

                // Assign the smart contract address to election register contract address.
                if (!string.IsNullOrEmpty(deployResult))
                {
                    election.RegisterContractAddress = deployResult;

                    var result = await _electionRepository.Update(election);

                    return result;
                }
            }

            return 0;
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

                if (election != null && !string.IsNullOrEmpty(election.ContractAddress))
                {
                    IVotingContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                    if (smartContractService != null)
                    {
                        var result = await smartContractService.Vote(voterProof, candidateId, election.ContractAddress);

                        // Insert a new entry in the UserVotes table to have an evidence is the user has voted or not.
                        if (result.IsSuccess && await _userVoteRepository.Update(user.Id, election.Id, true))
                        {
                            return new VoteResult(result.IsSuccess, result.Message);
                        }

                        return new VoteResult(false, "The proof is wrong or the user has already voted!");

                    }
                    return new VoteResult(false, "The smart contract service is null");
                }
            }
            return new VoteResult(false, "The candidate is null");
        }

        public async Task<int> GetElectionResult(DbElection election)
        {
            var result = await GetElectionVotes(election);

            return result;
        }

        #region Private

        private async Task<UserElection> GetElectionInternal(DbElection dbElection, DbUserVote? dbVote)
        {
            var election = new UserElection(dbElection)
            {
                HasVoted = dbVote?.HasVoted ?? false,
                HasRegistered = dbVote != null
            };

            election.NumberOfVotes = await GetElectionVotes(dbElection);

            return election;
        }

        private async Task<int> GetElectionVotes(DbElection election)
        {
            if (election.State == ElectionState.Completed)
            {
                IVotingContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                if (smartContractService != null && !string.IsNullOrEmpty(election.ContractAddress))
                {
                    return (await smartContractService.GetTotalNumberOfVotes(election.ContractAddress)).Value;
                }
            }

            return 0;
        }

        private async Task<List<int>> GetUserIds(DbElection election)
        {
            var usersVotes = await _userVoteRepository.GetAllForElection(election.Id);

            List<int> usersIds = usersVotes.Select(item => item.UserId).ToList();

            return usersIds;
        }

        #endregion
    }
}
