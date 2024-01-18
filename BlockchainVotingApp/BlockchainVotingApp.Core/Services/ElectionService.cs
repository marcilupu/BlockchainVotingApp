using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Microsoft.Extensions.Configuration;


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
                //todo: REFACTOR
                election.HasVoted = false; // await _smartContractService.HasUserVoted(user.Id, election.ContractAddress);
            }

            return elections;
        }

        public async Task<int> GetVoteForUser(int userId, string contractAddress)
        {
            //todo: REFACTOR
            var result = (Vote?)null; // await _smartContractService.GetUserVote(userId, contractAddress);

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

                ISmartContractService smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                if (smartContractService != null)
                {
                    election.NumberOfVotes = await smartContractService.GetTotalNumberOfVotes(election.ContractAddress);
                }
                else
                {
                    election.NumberOfVotes = 0;
                }
            }

            return elections.Where(x => x.HasVoted).ToList();
        }

        public async Task<bool> GenerateElectionSmartContract(DbElection election)
        {
            // Get users for this election
            var usersIds = await GetUserIds(election);

            // Get election context name
            string contextIdentifier = ElectionHelper.GetElectionContextIdentifier(election.Id, election.Name);

            // Generate election context
            var contractMetadata = await _smartContractGenerator.CreateSmartContractContext(contextIdentifier, usersIds);

            //Deploy a new smart contract to interact with
            var deployedContract = await _smartContractGenerator.DeploySmartContract(contextIdentifier, _smartContractConfiguration.AdminDefaultAccountPrivateKey);
            election.ContractAddress = deployedContract;

            //Add voters to smart contract
            //var result = await AddVoters(election, usersIds);

            //if (!result)
            //{
            //    return false;
            //}

            ////Add candidates to smart contract
            //var candidates = await _candidateRepository.GetAllForElection(election.Id);
            //foreach(var candidate in candidates)
            //{
            //    var candidateResult = await _smartContractService.AddCandidate(candidate.Id, election.ContractAddress);

            //    //If the smart contract add candidate failed, drop the candidate from the db
            //    if (!candidateResult)
            //    {
            //        await _candidateRepository.Delete(candidate);
            //        return false;
            //    }
            //}

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

        public async Task<bool> Vote(int userId, int candidateId)
        {
            var candidate = await _candidateService.Get(candidateId);

            if (candidate != null)
            {
                var election = await Get(candidate.ElectionId);

                if (election != null)
                {
                    //todo: REFACTOR
                    var result = false; // await _smartContractService.Vote(userId, candidateId, election.ContractAddress);

                    return result;
                }
            }

            return false;
        }


        #region Private

        private async Task<UserElection> RetrieveInternal(DbElection dbElection, int? userId)
        {
            var election = new UserElection(dbElection);

            if (userId.HasValue)
            {
                //todo: REFACTOR

                election.HasVoted = false; // await _smartContractService.HasUserVoted(userId.Value, election.ContractAddress);
            }
            else
            {
                election.HasVoted = false;
            }

            ISmartContractService smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

            if (smartContractService != null)
            {
                election.NumberOfVotes = await smartContractService.GetTotalNumberOfVotes(election.ContractAddress);
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
