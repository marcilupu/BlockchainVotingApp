using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Data.Repositories
{
    public interface IUserVoteRepository
    {
        Task<int> Insert(DbUserVote userVote);
        Task<int> Update(DbUserVote userVote);
        Task<int> AddOrUpdate(DbUserVote dbUserVote, bool isNew);

        Task<DbUserVote?> Get(int userId, int electionId);

        Task<List<DbUserVote>> GetAll(int userId);

        Task<DbUserVote?> Get(int id);
    }
}
