using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Elections;
using BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Election
{
    [Area("manage")]
    public class ElectionsController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IElectionRepository electionRepository)
        {
            var elections = await electionRepository.GetAll();

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
        public async Task<IActionResult> CreateElection([FromServices] IElectionRepository electionRepository,
                                                        AddElectionModel electionModel)
        {
            var election = electionModel.ToDb();

            await electionRepository.Insert(election);

            var elections = await electionRepository.GetAll();
            var electionViewModel = new ElectionsViewModel(elections);

            return View("/Areas/Manage/Views/Elections/Index.cshtml", electionViewModel);
        }
    }
}
