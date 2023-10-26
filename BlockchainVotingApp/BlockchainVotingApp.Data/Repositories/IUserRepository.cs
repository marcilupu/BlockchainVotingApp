﻿using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<DbUser?> GetByNationalId(string nationalId);
    }
}