using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.Elections.Include(item => item.County).Include(item => item.Candidates).FirstOrDefaultAsync(x => x.Id == electionId);
        }

        public async Task<List<DbElection>> GetAll()
        {
            return await _context.Elections.Include(item => item.County).Include(item => item.Candidates)
                                           .OrderBy(item => item.State)
                                           .OrderByDescending(x => x.StartDate).ToListAsync();
        }

        public async Task<List<DbElection>> GetAllByCounty(int countyId)
        {
            return await _context.Elections.Include(item => item.County).Include(item => item.Candidates)
                                            .Where(item => (item.CountyId.HasValue && item.CountyId == countyId) || !item.CountyId.HasValue)
                                            .OrderBy(item => item.State)
                                            .OrderByDescending(x => x.StartDate)
                                            .ToListAsync();
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
