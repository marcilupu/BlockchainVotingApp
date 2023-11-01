using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Candidates;
using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Net.WebSockets;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Candidate
{
    [Area("manage")]
    public class CandidatesController : Controller
    {
        public async Task<IActionResult> Index([FromServices] ICandidateService candidateService)
        {
            var candidates = await candidateService.GetAll();

            var candidatesViewModel = new CandidatesViewModel(candidates);

            return View(candidatesViewModel);
        }

        [HttpGet]
        public IActionResult AddCandidate() => View();

        [HttpPost]
        public async Task<IActionResult> AddCandidate([FromServices] ICandidateService candidateService,
                                                       AddCandidateModel addCandidateModel)
        {
            DbCandidate candidate = addCandidateModel.ToDb();

            await candidateService.Insert(candidate);

            return new OkResult();
        }
    }
}
