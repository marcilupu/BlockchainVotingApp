using BlockchainVotingApp.Models.Election.ViewModels;
using BlockchainVotingApp.Models.Register;

namespace BlockchainVotingApp.Models.Register
{
    public class RegisterViewModel
    {
        public RegisterViewModel(List<Core.DomainModels.UserElection> elections)
        {
            Items = elections.Select(item =>
            {
                return new RegisterItemViewModel(item);
            }).ToList();
        }
        public List<RegisterItemViewModel> Items { get; set; }
    }
}
