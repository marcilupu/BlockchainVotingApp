using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IAppUserService
    {
        Task<AppUser> GetUserAsync();
    }
}
