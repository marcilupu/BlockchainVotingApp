using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Data.Repositories
{
    public interface IUserVoteRepository
    {
        Task<int> Insert(DbUserVote userVote);
        
        Task<int> Update(DbUserVote userVote);
        
        Task<int> AddOrUpdate(DbUserVote dbUserVote, bool isNew);
        Task<bool> Update(int userId, int electionId, bool voted);

        Task<DbUserVote?> Get(int userId, int electionId);

        Task<List<DbUserVote>> GetAll(int userId, bool includeElection = false);

        Task<List<DbUserVote>> GetAllForElection(int electionId);

        Task<DbUserVote?> Get(int id);
    }
}
