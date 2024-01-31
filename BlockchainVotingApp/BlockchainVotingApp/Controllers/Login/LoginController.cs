using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.Models.Login;
using BlockchainVotingApp.Models.Login.ViewModels;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainVotingApp.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Register([FromServices] ICountyRepository countyRepository)
        {
            var counties = countyRepository.GetAll().Result;

            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Counties = new List<(string, int)>();

            foreach(DbCounty county in counties)
            {
                loginViewModel.Counties.Add((county.Name, county.Id));
            }

            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromServices] UserManager<DbUser> userManager,
                                                       RegisterModel registerModel,
                                                       [FromServices] IAppUserService appUserService)
        {
            if(ModelState.IsValid)
            {
                var users = await appUserService.GetAll();

                if(users.Any(x => x.NationalId == registerModel.NationalId))
                {
                    return BadRequest("The user with this national id already exists!");
                }

                var user = new DbUser()
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = $"{registerModel.FirstName.ToLower()}.{registerModel.LastName.ToLower()}",
                    Gender = registerModel.Gender,
                    NationaId = registerModel.NationalId,
                    Email = registerModel.Email,
                    CountyId = registerModel.County
                };

                var result = await userManager.CreateAsync(user, registerModel.Password);

                if(result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(user, "Voter");

                    return Redirect("/Login/Index");
                }    
            }
            return View("/Views/Login/Register.cshtml");
        }

        public async Task<IActionResult> Login([FromServices] IUserRepository userRepository, [FromServices] SignInManager<DbUser> signInManager, LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetByNationalId(loginModel.NationalId).Result;

                if(user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user.UserName, loginModel.Password, isPersistent:true, lockoutOnFailure: false);

                    if(result.Succeeded)
                    {
                        return Redirect("/Home/Index");
                    }
                }
            }
            return View("/Views/Login/Index.cshtml");
        }
    }
}
