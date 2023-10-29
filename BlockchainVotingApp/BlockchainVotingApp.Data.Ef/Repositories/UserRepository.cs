using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly VDbContext _vDbContext;

        public UserRepository(VDbContext vDbContext)
        {
            _vDbContext = vDbContext;
        }

        public Task<List<DbUser>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DbUser?> GetByNationalId(string nationalId)
        {
            return await _vDbContext.Users.FirstOrDefaultAsync(user => user.NationaId== nationalId);
        }
    }
}
