using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Models.Election.ViewModels;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ess;
using QRCoder;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Text.Unicode;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

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

        public IActionResult UploadProof()
        {
            var file = HttpContext.Request.Form.Files["file"];

            if (file != null)
            {
                using var stream = file.OpenReadStream();

#pragma warning disable CA1416 // Validate platform compatibility

                var bitmap = new Bitmap(stream);

                var luminanceSource = new BitmapLuminanceSource(bitmap);

                var source = new BinaryBitmap(new HybridBinarizer(luminanceSource));

#pragma warning restore CA1416 // Validate platform compatibility


                var reader = new QRCodeReader();

                var result = reader.decode(source);

                return new JsonResult(new { content = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.Text)) });
            }

            return new BadRequestResult();
        }

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
            if (candidateId == 0 || string.IsNullOrEmpty(proof))
            {
                return new BadRequestResult();
            }

            var user = await _appUserService.GetUserAsync();


            var result = await _electionService.Vote(Encoding.UTF8.GetString(Convert.FromBase64String(proof)), candidateId);

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

                if (candidate != null)
                {
                    var electionViewModel = new VoteViewModel(election, candidate.FullName);

                    return View("/Views/Election/VoteDetails.cshtml", electionViewModel);
                }
            }

            return NotFound();
        }

        //http method?
        public async Task<IActionResult> GenerateProof([FromServices] ISmartContractGenerator smartContractGenerator, int electionId)
        {
            var user = await _appUserService.GetUserAsync();
            var election = await _electionService.GetElectionForUser(electionId, user.Id);

            if (election != null)
            {
                var contextIdentifier = ElectionHelper.GetElectionContextIdentifier(election.Id, election.Name);
                var proof = await smartContractGenerator.GenerateProof(contextIdentifier, user.Id);

                string proofToString = JsonConvert.SerializeObject(proof);

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(proofToString, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);

                var imageBytes = qrCode.GetGraphic(12);

                return File(imageBytes, "image/jpeg", "proof.png");
            }

            return new BadRequestResult();
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
