using BlockchainVotingApp.Areas.Manage.Models.Register;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlockchainVotingApp.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class RegisterController : Controller
    {
        private readonly ICountyRepository _countyRepository;
        private readonly IElectionService _electionService;

        public RegisterController(ICountyRepository countyRepository, IElectionService electionService, IElectionRepository electionRepository)
        {
            _countyRepository = countyRepository;
            _electionService = electionService;
        }
        public async Task<IActionResult> Index()
        {
            var counties = await _countyRepository.GetAll();

            var elections = await _electionService.GetAll();

            var model = new RegisterViewModel(counties, elections);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateProof([FromServices] IRegisterService registerService,
                                                        CreateProofViewModel createProofViewModel)
        {
            var proof = await registerService.GenerateProof(createProofViewModel.Election, createProofViewModel.BirthYear, createProofViewModel.County);

            if (proof != null)
            {
                return File(proof, "image/jpeg", "proof.png");
            }

            return new BadRequestResult();
        }
    }
}
