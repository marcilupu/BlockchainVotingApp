using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BlockchainVotingApp.AppCode.Services.Users
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<DbUser> _userManager;

        public AppUserService(IHttpContextAccessor contextAccessor, UserManager<DbUser> userManager)
        {
            _contextAccessor = contextAccessor; 
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserAsync()
        { 
            return new AppUser(await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User));
        }
    }
}
