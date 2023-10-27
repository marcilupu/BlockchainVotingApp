using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Home
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index([FromServices] ICandidateRepository candidateRepository)
        {
            var candidates = await candidateRepository.GetAll();

            var homeViewModel = new HomeViewModel(candidates);

            return View(homeViewModel);
        }
    }
}