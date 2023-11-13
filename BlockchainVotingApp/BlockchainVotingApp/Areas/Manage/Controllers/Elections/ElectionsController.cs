using BlockchainVotingApp.AppCode.Extensions;
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

        public ElectionsController(ICountyRepository countyRepository, IElectionService electionService)
        {
            _countyRepository = countyRepository;
            _electionService = electionService;
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

            var result = await _electionService.Insert(election);

            if(result != 0)
            {
                var electionViewModel = await GetELectionsViewModel();

                return View("/Areas/Manage/Views/Elections/Index.cshtml", electionViewModel);
            }
            else
            {
                //Throw a message that the election could not be created and why...(eg. The election is ongoing and cannot be edited)
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromServices] IElectionRepository electionRepository, AddEditElectionModel electionModel, int electionId)
        {
            var dbDlection = await electionRepository.Get(electionId);

            if(dbDlection != null)
            {
                var election = electionModel.ToDb(dbDlection);
                var result = await electionRepository.Update(election);

                if (result != 0)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromServices] IElectionRepository electionRepository, int id)
        {
            var dbElection = await electionRepository.Get(id);

            if(dbElection != null )
            {
                var result = await electionRepository.Delete(dbElection);

                if(result)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }
    }
}
