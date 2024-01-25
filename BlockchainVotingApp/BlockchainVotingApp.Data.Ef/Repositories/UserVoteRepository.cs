using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Repositories
{
    internal class UserVoteRepository : IUserVoteRepository
    {
        private readonly VDbContext _context;
        public UserVoteRepository(VDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddOrUpdate(DbUserVote dbUserVote, bool isNew)
        {
            if(isNew)
            {
                await _context.AddAsync(dbUserVote);
            }
            else
            {
                _context.Update(dbUserVote);
            }

            await _context.SaveChangesAsync();

            return dbUserVote.Id;
        }

        public async Task<DbUserVote?> Get(int userId, int electionId)
        {
            return await _context.UserVotes.FirstOrDefaultAsync(x => x.UserId == userId && x.ElectionId == electionId);
        }

        public async Task<DbUserVote?> Get(int id)
        {
            return await _context.UserVotes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<DbUserVote>> GetAll(int userId)
        {
            return await _context.UserVotes.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<int> Insert(DbUserVote userVote)
        {
            await _context.AddAsync(userVote);

            await _context.SaveChangesAsync();

            return userVote.Id;
        }

        public async Task<int> Update(DbUserVote userVote)
        {
            _context.Update(userVote);

            await _context.SaveChangesAsync();

            return userVote.Id;
        }
    }
}
