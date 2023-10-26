using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Areas.Manage.Controllers.Election
{
    [Area("manage")]
    public class ElectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
