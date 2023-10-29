using BlockchainVotingApp.Controllers.Users;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Election.ViewModels;
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

            var electionViewModel = new ElectionsViewModel(elections);

            //check smart contract function HasUserVoted()
            //Set HasVoted


            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromServices] IElectionRepository electionRepository, int id)
        {
            var election = await electionRepository.Get(id);

            if (election != null)
            {
                var electionViewModel = new ElectionItemViewModel(election);

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
