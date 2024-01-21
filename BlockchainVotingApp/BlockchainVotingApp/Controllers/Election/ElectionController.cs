using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Models.Election.ViewModels;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Election
{
    [Authorize]
    public class ElectionController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IElectionService _electionService;
        public ElectionController(IAppUserService appUserService, IElectionService electionService)
        {
            _appUserService = appUserService;
            _electionService = electionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _appUserService.GetUserAsync();

            var elections = await _electionService.GetAllByCounty(user);

            var electionViewModel = new ElectionsViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _appUserService.GetUserAsync();

            var election = await _electionService.GetElectionForUser(id, user.Id);

            if (election != null)
            {
                if (election.HasVoted)
                {
                    return new UnauthorizedResult();
                }

                var electionViewModel = new ElectionCandidatesViewModel(election);

                return View("/Views/Election/Details.cshtml", electionViewModel);
            }

            return NotFound();
        }

        //ce metoda http este?
        public async Task<IActionResult> Vote(int candidateId)
        {
            var user = await _appUserService.GetUserAsync();

            var result = await _electionService.Vote(user.Id, candidateId);

            if (result)
            {
                var elections = await _electionService.GetAllByCounty(user);

                var electionViewModel = new VotesViewModel(elections);

                return View("/Views/Election/Votes.cshtml", electionViewModel);
            }

            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> VoteDetails([FromServices] ICandidateService candidateService, int electionId)
        {
            var election = await _electionService.Get(electionId);
            var user = await _appUserService.GetUserAsync();

            if (election != null)
            {
                var candidateId = await _electionService.GetVoteForUser(user.Id, election.ContractAddress);

                var candidate = await candidateService.Get(candidateId);

                if(candidate != null)
                {
                    var electionViewModel = new VoteViewModel(election, candidate.FullName);

                    return View("/Views/Election/VoteDetails.cshtml", electionViewModel);
                }
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Votes()
        {
            var user = await _appUserService.GetUserAsync();

            var elections = await _electionService.GetVotesForUser(user);

            var viewModel = new VotesViewModel(elections);

            return View(viewModel);
        }
    }
}
