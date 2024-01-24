using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace BlockchainVotingApp.AppCode.Services.Users
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<DbUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string _currentUserMemoryKey;
        private readonly string _currentUserRolesKey;

        public AppUserService(IHttpContextAccessor contextAccessor, UserManager<DbUser> userManager, IUserRepository userRepository, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _currentUserMemoryKey = configuration.GetSection("MemoryCacheKeys").GetSection("CurrentUserMemoryKey").ToString() ?? "currentUserKey";
            _currentUserRolesKey = configuration.GetSection("MemoryCacheKeys").GetSection("CurrentUserRolesMemoryKey").ToString() ?? "currentUserRolesKey";
        }

        public async Task<AppUser> ChangePassword(string currentPass, string newPass)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User);

            if (await _userManager.CheckPasswordAsync(user, currentPass))
            {
                await _userManager.ChangePasswordAsync(user, currentPass, newPass);
            }

            _memoryCache.Set(_currentUserMemoryKey, user);

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

        public async Task<DbUser> GetDbUserAsync()
        {
            if (_memoryCache.TryGetValue(_currentUserMemoryKey, out DbUser? currentUser))
            {
                return currentUser!;
            }

            var dbUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext?.User);

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));

            _memoryCache.Set(_currentUserMemoryKey, dbUser, cacheOptions);

            return dbUser;
        }

        public async Task<int> Update(DbUser dbUser)
        {
            return await _userRepository.Update(dbUser);
        }

        public async Task<AppUser> GetUserAsync()
        {
            return new AppUser(await GetDbUserAsync());
        }

        public async Task<IList<string>> GetUserRoles()
        {
            var user = await GetDbUserAsync();

            if (_memoryCache.TryGetValue(_currentUserRolesKey, out IList<string>? currentUserRoles))
            {
                return currentUserRoles!;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));

            _memoryCache.Set(_currentUserRolesKey, roles, cacheOptions);

            return roles;
        }
    }
}
