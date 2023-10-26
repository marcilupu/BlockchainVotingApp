using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Candidate
{
    [Area("manage")]
    public class CandidateController: Controller
    {
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult AddCandidate() => View();

        [HttpPost]
        public IActionResult AddCandidate([FromServices] ICandidateRepository candidateRepository) => View();
    }
}
