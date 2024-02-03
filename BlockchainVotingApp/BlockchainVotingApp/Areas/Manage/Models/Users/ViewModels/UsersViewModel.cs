using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Areas.Manage.Models.Users.ViewModels
{
    public class UsersViewModel
    {
        public UsersViewModel(List<AppUser> users)
        {
            Users = users.Select(item =>
            {
                return new UserViewModel(item);
            }).ToList();
        }

        public List<UserViewModel> Users;
    }

    public class UserViewModel
    {
        public UserViewModel(AppUser appUser) {
            FullName = appUser.Username;
            Email= appUser.Email;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string CountyName { get; set; } = null!;
    }
}
