using BlockchainVotingApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Users
{
    public abstract class AuthenticatedController : Controller
    {
        protected readonly UserManager<DbUser> _userManager;

        public AuthenticatedController(UserManager<DbUser> userManager)
        {
            this._userManager = userManager;
        }

        protected string GetUserId()
        {
            return _userManager.GetUserId(User);
        }
    }
}
