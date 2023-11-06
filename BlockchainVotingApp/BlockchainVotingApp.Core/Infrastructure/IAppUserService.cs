using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IAppUserService
    {
        Task<AppUser> ChangePassword(string currentPass, string newPass); 
        Task<AppUser> GetUserAsync();

        Task<IList<string>> GetUserRoles();

        Task<List<AppUser>> GetAll();
    }
}
