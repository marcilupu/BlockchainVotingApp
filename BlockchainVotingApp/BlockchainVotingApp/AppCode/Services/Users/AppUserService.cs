using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BlockchainVotingApp.AppCode.Services.Users
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<DbUser> _userManager;
        private readonly IUserRepository _userRepository;

        public AppUserService(IHttpContextAccessor contextAccessor, UserManager<DbUser> userManager, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor; 
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<AppUser> ChangePassword(string currentPass, string newPass)
        {
            var user =  await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User);

            if(await _userManager.CheckPasswordAsync(user, currentPass))
            {
                await _userManager.ChangePasswordAsync(user, currentPass, newPass);
            }

            return new AppUser(user);
        }

        public async Task<List<AppUser>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return users.Select(item =>
            {
                return new AppUser(item);
            }).ToList();
        }

        public async Task<AppUser> GetUserAsync()
        { 
            return new AppUser(await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User));
        }

        public async Task<IList<string>> GetUserRoles()
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User);
            return await _userManager.GetRolesAsync(user);
        }
    }
}
