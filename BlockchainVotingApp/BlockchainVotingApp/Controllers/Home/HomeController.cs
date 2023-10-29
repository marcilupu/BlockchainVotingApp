using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {           
            return View();
        }
    }
}