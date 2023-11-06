using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BlockchainVotingApp.Controllers.User
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAppUserService _appUserService;

        public UserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        private async Task<UserViewModel> CreateUserViewModel([FromServices] ICountyRepository countyRepository)
        {
            var user = await _appUserService.GetUserAsync();

            var viewModel = new UserViewModel(user);

            return viewModel;
        }

        public async Task<IActionResult> Index([FromServices] ICountyRepository countyRepository)
        {
            var viewModel = await CreateUserViewModel(countyRepository);

            return View(viewModel);
        }

        public async Task<IActionResult> ChangePassword([FromServices] ICountyRepository countyRepository, string currentPassword, string newPassword)
        {
            var user = await _appUserService.ChangePassword(currentPassword, newPassword);

            var viewModel = await CreateUserViewModel(countyRepository);

            //return message with password was succesfully changed
            return View("/Views/User/Index.cshtml",viewModel);
        }

        public async Task<IActionResult> Logout([FromServices] SignInManager<DbUser> signInManager)
        {
            await signInManager.SignOutAsync();

            return Redirect("/Login/Index");
        }
    }
}
