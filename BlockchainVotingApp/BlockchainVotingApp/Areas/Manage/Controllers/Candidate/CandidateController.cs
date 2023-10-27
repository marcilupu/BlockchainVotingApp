using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Candidate;
using BlockchainVotingApp.Areas.Manage.Models.Candidate.ViewModels;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Candidate
{
    [Area("manage")]
    public class CandidateController: Controller
    {
        public async Task<IActionResult> Index([FromServices] ICandidateRepository candidateRepository) 
        {
            var candidates = await candidateRepository.GetAll();

            var candidatesViewModel = new CandidateViewModel(candidates);

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
