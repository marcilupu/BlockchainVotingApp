using BlockchainVotingApp.AppCode.Extensions;
using BlockchainVotingApp.Areas.Manage.Models.Candidates;
using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Net.WebSockets;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Candidate
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class CandidatesController : Controller
    {
        public async Task<IActionResult> Index([FromServices] ICandidateService candidateService)
        {
            var candidates = await candidateService.GetAll();

            var candidatesViewModel = new CandidatesViewModel(candidates);

            return View(candidatesViewModel);
        }

        [HttpGet]
        public IActionResult AddCandidate() => View();

        [HttpPost]
        public async Task<IActionResult> AddCandidate([FromServices] ICandidateService candidateService,
                                                       AddEditCandidateModel addCandidateModel)
        {
            DbCandidate candidate = addCandidateModel.ToDb();

            var result = await candidateService.Insert(candidate);

            if(result != 0)
            {
                return new OkResult();
            }
            else
            {
                //return a message that the candidate could not be added and why
                return new BadRequestResult();
            }    
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromServices] ICandidateRepository candidateRepository, AddEditCandidateModel model, int candidateId)
        {
            var dbDlection = await candidateRepository.Get(candidateId);

            if (dbDlection != null)
            {
                var candidate = model.ToDb(dbDlection);
                var result = await candidateRepository.Update(candidate);

                if (result != 0)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromServices] ICandidateRepository candidateRepository, int id)
        {
            var dbCandidate = await candidateRepository.Get(id);

            if (dbCandidate != null)
            {
                var result = await candidateRepository.Delete(dbCandidate);

                if (result)
                {
                    return new OkResult();
                }
            }
            return new BadRequestResult();
        }
    }
}
