using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Models.User.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(AppUser appUser)
        {
            FullName = appUser.Name;
            Email = appUser.Email;
        }

        public string FullName { get; set; }
        public string Email { get; set; }

    }
}
