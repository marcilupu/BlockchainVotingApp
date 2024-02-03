using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Models.Election.ViewModels;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


namespace BlockchainVotingApp.Controllers
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

        #region Elections
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _appUserService.GetUserAsync();

            var elections = await _electionService.GetAll(user);

            var electionViewModel = new ElectionsViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _appUserService.GetUserAsync();

            var election = await _electionService.GetUserElection(id, user);

            if (election != null)
            {
                var electionViewModel = new ElectionCandidatesViewModel(election);

                return View("/Views/Election/Details.cshtml", electionViewModel);
            }

            return NotFound();
        }
        #endregion

        #region Proof

        [HttpPost]
        public IActionResult UploadProof([FromServices] IQRGenerator qRGenerator)
        {
            var file = HttpContext.Request.Form.Files["file"];

            if (file != null)
            {
                using var stream = file.OpenReadStream();

                var result = qRGenerator.GetCodeContent(stream);

                return new JsonResult(new { content = result });
            }

            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateProof([FromServices] IVotingContractGenerator smartContractGenerator, [FromServices] IQRGenerator qRGenerator, int electionId)
        {
            var user = await _appUserService.GetUserAsync();

            var election = await _electionService.GetUserElection(electionId, user);

            if (election != null)
            {
                var contextIdentifier = ElectionHelper.CreateContextIdentifier(election.Id, election.Name);

                var proof = await smartContractGenerator.GenerateProof(contextIdentifier, user.Id);

                if (proof != null)
                {
                    string proofToString = JsonConvert.SerializeObject(proof);
        
                    var imageBytes = qRGenerator.CreateCode(proofToString);

                    return File(imageBytes, "image/jpeg", "proof.png");
                }
            }

            return new BadRequestResult();
        }
        #endregion

        #region Vote
        [HttpGet]
        public async Task<IActionResult> GetVoteModal([FromServices] ICandidateService candidateService, int electionId)
        {
            var candidates = await candidateService.GetAllForElection(electionId);

            var model = new VoteModalViewModel(candidates);

            return PartialView("/Views/Election/_VoteModal.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int candidateId, string proof)
        { 
            if (candidateId != 0 && !string.IsNullOrEmpty(proof))
            {
                var user = await _appUserService.GetUserAsync();

                var result = await _electionService.Vote(user, Encoding.UTF8.GetString(Convert.FromBase64String(proof)), candidateId);

                if (result.Success)
                {   
                    var elections = await _electionService.GetAll(user);

                    var electionViewModel = new VotesViewModel(elections);

                    return View("/Views/Election/Votes.cshtml", electionViewModel);
                }

                return BadRequest(result.ErrorMessage);
            }

            return BadRequest("The candidate or the proof is null");
        }

        [HttpGet]
        public async Task<IActionResult> Votes()
        {
            var user = await _appUserService.GetUserAsync();

            // Retrieve all elections for which the user has voted.
            var elections = (await _electionService.GetAll(user)).Where(x => x.HasVoted).ToList();

            var viewModel = new VotesViewModel(elections);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VoteDetails([FromServices] ICandidateService candidateService, int electionId, string proof)
        {
            var user = await _appUserService.GetUserAsync();

            var candidateId = await _electionService.GetUserVote(user, Encoding.UTF8.GetString(Convert.FromBase64String(proof)), electionId);

            if (candidateId != null)
            {
                var candidate = await candidateService.Get(candidateId.Value);

                if(candidate != null)
                {
                    var election = await _electionService.GetUserElection(electionId, user);

                    if (election != null)
                    {
                        var electionViewModel = new VoteViewModel(election!, candidate!.FullName);

                        return View("/Views/Election/VoteDetails.cshtml", electionViewModel);
                    }
                }
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult GetProofModal()
        {
            return PartialView("/Views/Election/_ProofModal.cshtml");
        }
        #endregion
    }
}
