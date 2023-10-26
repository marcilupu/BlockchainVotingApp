using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers.Home
{
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger) { }

        public IActionResult Index()
        {
            return View();
        }
    }
}