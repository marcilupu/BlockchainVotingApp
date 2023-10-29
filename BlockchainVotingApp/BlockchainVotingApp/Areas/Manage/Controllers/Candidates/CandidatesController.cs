using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Candidates;
using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Candidate
{
    [Area("manage")]
    public class CandidatesController: Controller
    {
        public async Task<IActionResult> Index([FromServices] ICandidateRepository candidateRepository) 
        {
            var candidates = await candidateRepository.GetAll();

            var candidatesViewModel = new CandidatesViewModel(candidates);

            return View(candidatesViewModel);
        }

        [HttpGet]
        public IActionResult AddCandidate() => View();

        [HttpPost]
        public async Task<IActionResult> AddCandidate([FromServices] ICandidateRepository candidateRepository, AddCandidateModel addCandidateModel)
        {
            DbCandidate candidate = addCandidateModel.ToDb();

            await candidateRepository.Insert(candidate);

            return new OkResult();
        }
    }
}
