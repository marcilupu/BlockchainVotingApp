using BlockchainVotingApp.Data.Models;


namespace BlockchainVotingApp.Data.Repositories
{
    public interface ICountyRepository
    {
        public Task<List<DbCounty>> GetAll();

        public Task<DbCounty?> Get(int id);
    }
}
