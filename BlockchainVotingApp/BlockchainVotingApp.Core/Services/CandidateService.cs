using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace BlockchainVotingApp.Core.Services
{
    internal class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IElectionRepository _electionRepository;

        public CandidateService(ICandidateRepository candidateRepository, IElectionRepository electionRepository)
        {
            _candidateRepository = candidateRepository;
            _electionRepository = electionRepository;
        }
        public async Task<Candidate?> Get(int id)
        {
            var dbCandidate = await _candidateRepository.Get(id);

            if(dbCandidate != null)
            {
                return new Candidate(dbCandidate);
            }
            else
            {
                return null;
            }
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
