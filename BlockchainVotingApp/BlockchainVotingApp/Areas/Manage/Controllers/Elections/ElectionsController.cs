using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Areas.Manage.Models.Elections;
using BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Election
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class ElectionsController : Controller
    {
        private readonly ICountyRepository _countyRepository;
        private readonly IElectionService _electionService;
        private readonly IElectionRepository _electionRepository;

        public ElectionsController(ICountyRepository countyRepository, IElectionService electionService, IElectionRepository electionRepository)
        {
            _countyRepository = countyRepository;
            _electionService = electionService;
            _electionRepository = electionRepository;
        }

        private async Task<ElectionsViewModel> GetELectionsViewModel()
        {
            var elections = await _electionService.GetAll();

            var counties = await _countyRepository.GetAll();

            var electionViewModel = new ElectionsViewModel(elections, counties);

            return electionViewModel;
        }

        public async Task<IActionResult> Index()
        {
            var electionViewModel = await GetELectionsViewModel();

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateElection()
        {
            var counties = await _countyRepository.GetAll();

            AddElectionViewModel electionViewModel = new(counties);

            return View("/Areas/Manage/Views/Elections/AddElection.cshtml", electionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateElection(AddEditElectionModel electionModel)
        {
            var election = electionModel.ToDb();

            var result = await _electionService.Insert(election, electionModel.County);
            if (result != 0)
            {
                var electionViewModel = await GetELectionsViewModel();

                return View("/Areas/Manage/Views/Elections/Index.cshtml", electionViewModel);
            }

            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditElectionModel electionModel, int electionId)
        {
            var dbDlection = await _electionRepository.Get(electionId);

            if (dbDlection != null)
            {
                await _electionService.ChangeElectionState(dbDlection, electionModel.State);

                var election = electionModel.ToDb(dbDlection);

                var result = await _electionRepository.Update(election);

                if (result != 0)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbElection = await _electionRepository.Get(id);

            if (dbElection != null)
            {
                var result = await _electionRepository.Delete(dbElection);

                if (result)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSmartContract(int electionId)
        {
            var dbElection = await _electionRepository.Get(electionId);

            if (dbElection != null && dbElection.State == ElectionState.Upcoming)
            {
                var result = await _electionService.InitializeElectionContext(dbElection);

                if (result)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }
    }
}
