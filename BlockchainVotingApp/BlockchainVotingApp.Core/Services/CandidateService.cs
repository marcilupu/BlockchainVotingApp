using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace BlockchainVotingApp.Core.Services
{
    internal class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly ISmartContractServiceFactory _smartContractServiceFactory;
        private readonly ISmartContractGenerator _smartContractGenerator;

        public CandidateService(ICandidateRepository candidateRepository, IElectionRepository electionRepository, ISmartContractServiceFactory smartContractServiceFactory, ISmartContractGenerator smartContractGenerator)
        {
            _candidateRepository = candidateRepository;
            _electionRepository = electionRepository;
            _smartContractServiceFactory = smartContractServiceFactory;
            _smartContractGenerator = smartContractGenerator;
        }
        public async Task<Candidate?> Get(int id)
        {
            var dbCandidate = await _candidateRepository.Get(id);

            if (dbCandidate != null)
            {
                return new Candidate(dbCandidate);
            }
            else
            {
                return null;
            }
        }

        public async Task<int?> GetCandidateResult(DbCandidate candidate)
        {
            var election = await _electionRepository.Get(candidate.ElectionId);
            if (election != null && !string.IsNullOrEmpty(election.ContractAddress))
            {
                ISmartContractService? smartContractService = await ElectionHelper.CreateSmartContractService(_smartContractServiceFactory, _smartContractGenerator, election.Id, election.Name);

                if (smartContractService != null)
                {
                    var result = await smartContractService.GetCandidateResult(candidate.Id, election.ContractAddress);

                    if (result.Value != null)
                        return result.Value.Result;
                }
            }
            return null;
        }

        public async Task<List<Candidate>> GetAll()
        {
            var dbCandidates = await _candidateRepository.GetAll();
            var candidates = dbCandidates.Select(item =>
            {
                return new Candidate(item);
            }).ToList();

            return candidates;
        }

        public async Task<List<Candidate>> GetAllForElection(int electionId)
        {
            var dbCandidates = await _candidateRepository.GetAllForElection(electionId);

            var candidates = dbCandidates.Select(item =>
            {
                return new Candidate(item);
            }).ToList();
            return candidates;
        }


        public async Task<int> Insert(DbCandidate dbCandidate)
        {
            int candidateId = await _candidateRepository.Insert(dbCandidate);

            return candidateId;
        }

        public async Task<int> Update(DbCandidate dbCandidate)
        {
            return await _candidateRepository.Update(dbCandidate);
        }
    }
}
