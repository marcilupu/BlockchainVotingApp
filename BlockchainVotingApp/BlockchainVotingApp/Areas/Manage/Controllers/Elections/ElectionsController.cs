using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Elections;
using BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Election
{
    [Area("manage")]
    public class ElectionsController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IElectionService electionService)
        {
            var elections = await electionService.GetAll();

            var electionViewModel = new ElectionsViewModel(elections);

            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateElection([FromServices] ICountyRepository countyRepository)
        {
            var counties = await countyRepository.GetAll();

            AddElectionViewModel electionViewModel = new(counties);

            return View("/Areas/Manage/Views/Elections/AddElection.cshtml", electionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateElection([FromServices] IElectionService electionService,
                                                        AddElectionModel electionModel)
        {
            var election = electionModel.ToDb();

            await electionService.Insert(election);

            var elections = await electionService.GetAll();
            var electionViewModel = new ElectionsViewModel(elections);

            return View("/Areas/Manage/Views/Elections/Index.cshtml", electionViewModel);

        }
    }
}
