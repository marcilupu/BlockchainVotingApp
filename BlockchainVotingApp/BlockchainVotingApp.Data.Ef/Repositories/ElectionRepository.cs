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

        public async Task<DbElection?> Get(int electionId)
        {
            return await _context.Elections.Include(item => item.County).FirstOrDefaultAsync();
        }

        public async Task<List<DbElection>> GetAll()
        {
            return await _context.Elections.Include(item => item.County).ToListAsync();
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
