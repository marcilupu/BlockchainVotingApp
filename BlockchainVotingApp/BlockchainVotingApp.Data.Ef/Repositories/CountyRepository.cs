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
    internal class CountyRepository : ICountyRepository
    {
        private readonly VDbContext _vDbContext;

        public CountyRepository(VDbContext vDbContext)
        {
            _vDbContext = vDbContext;
        }

        public async Task<List<DbCounty>> GetAll()
        {
            return await _vDbContext.Counties.ToListAsync();
        }
    }
}
