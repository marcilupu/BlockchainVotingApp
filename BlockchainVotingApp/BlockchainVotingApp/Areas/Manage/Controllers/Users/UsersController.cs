using BlockchainVotingApp.Areas.Manage.Models.Users.ViewModels;
using BlockchainVotingApp.Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Users
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IAppUserService _appUserService;
        public UsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _appUserService.GetAll();

            var viewModel = new UsersViewModel(users);

            return View(viewModel);
        }
    }
}
