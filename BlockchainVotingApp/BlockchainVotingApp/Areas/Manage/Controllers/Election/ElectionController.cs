using BlockchainVotingApp.Areas.Manage.Models.Election;
using BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Signer.Crypto;
using System.Reflection.Metadata.Ecma335;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Election
{
    [Area("manage")]
    public class ElectionController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IElectionRepository electionRepository)
        {
            var elections = await electionRepository.GetAll();

            var electionViewModel = new ElectionViewModel(elections);
   
            return View(electionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateElection([FromServices] ICountyRepository countyRepository)
        {
            AddElectionViewModel electionViewModel = new AddElectionViewModel();

            var counties = await countyRepository.GetAll();

            electionViewModel.Counties = counties.Select(county =>
            {
                return (county.Name, county.Id);
            }).ToList();

            return View("/Areas/Manage/Views/Election/AddElection.cshtml", electionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateElection([FromServices] IElectionRepository electionRepository, AddElectionModel electionModel)
        {
            
            var election = new DbElection()
            {
                Name = electionModel.Name,
                ContractAddress = electionModel.ContractAddress,
                StartDate = electionModel.StartDate,
                EndDate = electionModel.EndDate,
                Rules = electionModel.Rules,
                CountyId = electionModel.County,
                State = ElectionState.Upcoming
            };

            int electionId = await electionRepository.Insert(election);

            return View("/Areas/Manage/Views/Election/Index.cshtml");
        }
    }
}
