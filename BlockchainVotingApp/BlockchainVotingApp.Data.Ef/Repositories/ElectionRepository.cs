using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Repositories
{
    internal class ElectionRepository : IElectionRepository
    {
        private readonly VDbContext _context;
        public ElectionRepository(VDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(DbElection election)
        {
            _context.Elections.Remove(election);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DbElection?> Get(int electionId)
        {
            return await _context.Elections.Include(item => item.Candidates).FirstOrDefaultAsync(x => x.Id == electionId);
        }

        public async Task<List<DbElection>> Get(int[] electionIds)
        {
            return await _context.Elections.Where(x => electionIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<DbElection>> GetAll()
        {
            return await _context.Elections.Include(item => item.Candidates)
                                           .OrderBy(item => item.State)
                                           .OrderByDescending(x => x.StartDate).ToListAsync();
        }

        public async Task<int> Insert(DbElection election)
        {
            await _context.AddAsync(election);

            await _context.SaveChangesAsync();

            return election.Id;
        }

        public async Task<int> Update(DbElection election)
        {
            _context.Update(election);

            await _context.SaveChangesAsync();

            return election.Id;
        }


    }
}
