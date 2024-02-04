using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BlockchainVotingApp.Controllers
{
    [Authorize]
    public class RegisterController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IElectionService _electionService;
        private readonly IUserVoteRepository _userVoteRepository;

        public RegisterController(IAppUserService appUserService, IElectionService electionService, IUserVoteRepository userVoteRepository)
        {
            _appUserService = appUserService;
            _electionService = electionService;
            _userVoteRepository = userVoteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _appUserService.GetUserAsync();

            var elections = await _electionService.GetAll(user);

            var electionViewModel = new RegisterViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public IActionResult GetProofModal()
        {
            return PartialView("/Views/Register/_ProofModal.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProof([FromServices] IQRGenerator qRGenerator, [FromServices] IRegisterService registerService, int electionId)
        {
            var file = HttpContext.Request.Form.Files["file"];

            if (file != null)
            {
                using var stream = file.OpenReadStream();

                var result = qRGenerator.GetCodeContent(stream);

                var user = await _appUserService.GetUserAsync();

                var registerSucceeded = await registerService.Register(user, electionId, Encoding.UTF8.GetString(Convert.FromBase64String(result)));

                if (registerSucceeded)
                {
                    return new JsonResult(new { content = result });
                }
            }

            return new BadRequestResult();
        }
    }
}
