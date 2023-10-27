using BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels;
using BlockchainVotingApp.Controllers.Users;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Election;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Election
{
    public class ElectionController : AuthenticatedController
    {
        public ElectionController(UserManager<DbUser> userManager) : base(userManager) { }

        public async Task<IActionResult> Index([FromServices] IElectionRepository electionRepository)

        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var elections = await electionRepository.GetAllByCounty(user.CountyId);

            var electionViewModel = new ElectionsViewModels(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromServices] IElectionRepository electionRepository, int id)
        {
            var election = await electionRepository.Get(id);

            var electionViewModel = new ElectionItemViewModel(election!);

            return View("/Views/Election/Details.cshtml", electionViewModel);
        }

        public IActionResult Vote()
        { 
            return new OkResult();
        }
    }
}
