using BlockchainVotingApp.AppCode.Services.Users;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Election.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Election
{
    public class ElectionController : Controller
    {
        public ElectionController(UserManager<DbUser> userManager) { }

        public async Task<IActionResult> Index([FromServices] IElectionService electionService, [FromServices] IAppUserService appUserService)
        {
            var user = await appUserService.GetUserAsync();

            var elections = await  electionService.GetAllByCounty(user.CountyId);

            var electionViewModel = new ElectionsViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromServices] IElectionService electionService, int id)
        {
            var election = await electionService.Get(id);

            if (election != null)
            {
                var electionViewModel = new ElectionCandidatesViewModel(election);

                return View("/Views/Election/Details.cshtml", electionViewModel);
            }

            return NotFound();
        }

        public IActionResult Vote(int candidateId)
        {
            

            return new OkResult();
        }
    }
}
