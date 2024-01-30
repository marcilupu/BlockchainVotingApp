using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace BlockchainVotingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IElectionService _electionService;
        private readonly IElectionRepository _electionRepository;
        private ICandidateRepository _candidateRepository;
        private readonly ICandidateService _candidateService;

        public HomeController(IElectionService electionService, ICandidateRepository candidateRepository, ICandidateService candidateService, IElectionRepository electionRepository)
        {
            _electionService = electionService;
            _candidateRepository = candidateRepository;
            _candidateService = candidateService;
            _electionRepository = electionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var elections = (await _electionService.GetAll()).Where(x => x.State == Data.Models.ElectionState.Completed);

            var viewModel = new HomeViewModel(elections.ToList());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates(int electionId)
        {
            if(electionId != 0)
            {
                var candidates = await _candidateRepository.GetAllForElection(electionId);

                var election = await _electionRepository.Get(electionId);

                if (election != null)
                {
                    var electionResult = await _electionService.GetElectionResult(election);

                    IDictionary<string, int> candidatesDict = new Dictionary<string, int>();

                    foreach (var candidate in candidates)
                    {
                        var fullName = $"{candidate.FirstName} {candidate.LastName}";
                        var result = await _candidateService.GetCandidateResult(candidate);

                        candidatesDict.Add(fullName, result == null ? 0 : result.Value);
                    }

                    return new JsonResult(new { candidatesDict, electionResult });
                }
            }

            return new BadRequestResult();
        }
    }
}