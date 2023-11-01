using BlockchainVotingApp.AppCode.Services.Users;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Election.ViewModels;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Election
{
    public class ElectionController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IElectionService _electionService;
        public ElectionController(IAppUserService appUserService, IElectionService electionService)
        {
            _appUserService = appUserService;
            _electionService = electionService;
        }

        public async Task<IActionResult> Index([FromServices] ISmartContractService smartContractService)
        {
            var user = await _appUserService.GetUserAsync();

            var elections = await _electionService.GetAllByCounty(user);          

            var electionViewModel = new ElectionsViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var election = await _electionService.Get(id);

            if (election != null)
            {
                var electionViewModel = new ElectionCandidatesViewModel(election);

                return View("/Views/Election/Details.cshtml", electionViewModel);
            }

            return NotFound();
        }

        public async Task<IActionResult> Vote(int candidateId)
        {
            var user = await _appUserService.GetUserAsync();

            var result = await _electionService.Vote(user.Id, candidateId);

            if(result)
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
    }
}
