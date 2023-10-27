using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Repositories
{
    internal class CandidateRepository : ICandidateRepository
    {
        private readonly VDbContext _context;
        public CandidateRepository(VDbContext context)
        {
            _context = context;
        }

        public async Task<DbCandidate?> Get(int id)
        {
            return await _context.Candidates.Include(item => item.Election).FirstOrDefaultAsync();
        }

        public async Task<List<DbCandidate>> GetAll()
        {
            return await _context.Candidates.Include(item => item.Election).ToListAsync();
        }

        public async Task<List<DbCandidate>> GetAllForElection(int electionId)
        {
            return await _context.Candidates.Include(item => item.Election).Where(item => item.ElectionId == electionId).ToListAsync();
        }

        public async Task<int> Insert(DbCandidate dbCandidate)
        {
            await _context.Candidates.AddAsync(dbCandidate);

            await _context.SaveChangesAsync();

            return dbCandidate.Id;
        }

        public async Task<int> Update(DbCandidate dbCandidate)
        {
            _context.Candidates.Update(dbCandidate);

            await _context.SaveChangesAsync();

            return dbCandidate.Id;
        }
    }
}
